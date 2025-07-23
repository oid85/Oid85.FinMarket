using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Domain.Models.StatisticalArbitration;
using Accord.Statistics.Models.Regression.Linear;
using MathNet.Numerics.LinearAlgebra;
using Newtonsoft.Json;
using NLog;
using Oid85.FinMarket.Common.Utils;

namespace Oid85.FinMarket.Application.Services;

public class StatisticalArbitrageService(
    ITickerListUtilService tickerListUtilService,
    IDailyCandleRepository dailyCandleRepository,
    ICorrelationRepository correlationRepository,
    IRegressionTailRepository regressionTailRepository,
    ILogger logger) 
    : IStatisticalArbitrageService
{
    /// <inheritdoc />
    public async Task CalculateCorrelationAsync()
    {
        var shares = await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.StatisticalArbitrageShares);
        var futures = (await tickerListUtilService.GetFuturesByTickerListAsync(KnownTickerLists.StatisticalArbitrageFutures))
            .Where(x => x.ExpirationDate > DateOnly.FromDateTime(DateTime.Today)).ToList();
        
        var candles = new Dictionary<string, List<DailyCandle>>();
        
        var from = DateOnly.FromDateTime(DateTime.Today.AddYears(-1));
        var to = DateOnly.FromDateTime(DateTime.Today);
        
        foreach (var share in shares)
            candles.TryAdd(share.Ticker, await dailyCandleRepository.GetAsync(share.Ticker, from, to));
        
        foreach (var future in futures)
            candles.TryAdd(future.Ticker, await dailyCandleRepository.GetAsync(future.Ticker, from, to));
        
        List<string> tickers = 
        [
            ..shares.Select(x => x.Ticker),
            ..futures.Select(x => x.Ticker)
        ];

        // Очистим таблицу
        var correlations = await correlationRepository.GetAllAsync();
        
        foreach (var correlation in correlations)
            await correlationRepository.UpdateAsync(correlation.Ticker1, correlation.Ticker2, 0.0);
        
        for (int i = 0; i < tickers.Count; i++)
        {
            for (int j = i + 1; j < tickers.Count; j++)
            {
                try
                {
                    if (candles[tickers[i]].Count == 0 || candles[tickers[j]].Count == 0)
                        continue;
                    
                    // Получаем свечи и синхронизируем массивы по дате
                    var syncCandles = SyncCandles(candles[tickers[i]], candles[tickers[j]]);
                    
                    var prices1 = syncCandles.Candles1.Select(x => x.Close).ToList();
                    var prices2 = syncCandles.Candles2.Select(x => x.Close).ToList();
                    
                    // Логарифмируем
                    var logValues1 = prices1.Log();
                    var logValues2 = prices2.Log();
                    
                    // Центрируем
                    var centeringValues1 = logValues1.Centering();
                    var centeringValues2 = logValues2.Centering();

                    // Делим на стандартное отклонение
                    var divStdValues1 = centeringValues1.DivConst(centeringValues1.StdDev());
                    var divStdValues2 = centeringValues2.DivConst(centeringValues2.StdDev());
                    
                    // Приращения
                    var incrementValues1 = divStdValues1.Increments();
                    var incrementValues2 = divStdValues2.Increments();
                    
                    // Расчет корреляции
                    double correlation = incrementValues1.Correlation(incrementValues2);
                    
                    var correlationModel = new Correlation
                    {
                        Ticker1 = tickers[i],
                        Ticker2 = tickers[j],
                        Value = correlation
                    };
                    
                    await correlationRepository.AddAsync(correlationModel);
                }
                
                catch (Exception exception)
                {
                    logger.Error(exception, "Ошибка расчета корреляции. {ticker1}, {ticker2}", tickers[i], tickers[j]);
                }
            }
        }
    }

    /// <inheritdoc />
    public async Task<Dictionary<string, RegressionTail>> CalculateRegressionTailsAsync()
    {
        var correlations = (await correlationRepository.GetAllAsync())
            .Where(x => x.Value is >= 0.8 and < 1.0).ToList();
        
        var from = DateOnly.FromDateTime(DateTime.Today.AddYears(-1));
        var to = DateOnly.FromDateTime(DateTime.Today);
        
        var tails = new Dictionary<string, RegressionTail>();
        
        foreach (var correlation in correlations)
        {
            try
            {
                // Получаем и синхронизируем свечи
                var candles1 = await dailyCandleRepository.GetAsync(correlation.Ticker1, from, to);
                var candles2 = await dailyCandleRepository.GetAsync(correlation.Ticker2, from, to);
                var syncCandles = SyncCandles(candles1, candles2);
            
                // Declare some sample test data.
                double[] inputs = syncCandles.Candles2.Select(x => x.Close).ToArray();
                double[] outputs = syncCandles.Candles1.Select(x => x.Close).ToArray();

                // Use Ordinary Least Squares to learn the regression
                var ols = new OrdinaryLeastSquares();

                // Use OLS to learn the simple linear regression
                SimpleLinearRegression regression = ols.Learn(inputs, outputs);

                // We can also extract the slope and the intercept term for the line
                double slope = regression.Slope;
                double intercept = regression.Intercept;
            
                // Расчет хвостов
                for (int i = 0; i < syncCandles.Candles1.Count; i++)
                {
                    double y = slope * syncCandles.Candles2[i].Close + intercept;
                    double tailValue = syncCandles.Candles1[i].Close - y;

                    string key = $"{correlation.Ticker1},{correlation.Ticker2}";
                
                    if (!tails.ContainsKey(key))
                        tails.Add(key, new RegressionTail { Ticker1 = correlation.Ticker1, Ticker2 = correlation.Ticker2 });
                
                    tails[key].Tails.Add(tailValue);
                }
            }
            
            catch (Exception exception)
            {
                logger.Error(exception, "Ошибка расчета остатков регрессии. {ticker1}, {ticker2}", correlation.Ticker1, correlation.Ticker2);
            }
        }
        
        // Сохраняем хвосты
        foreach (var tail in tails)
            await regressionTailRepository.AddAsync(tail.Value);
        
        return tails;
    }

    private static (List<DailyCandle> Candles1, List<DailyCandle> Candles2) SyncCandles(List<DailyCandle> candles1, List<DailyCandle> candles2)
    {
        var dates1 = candles1.Select(x => x.Date).ToList();
        var dates2 = candles2.Select(x => x.Date).ToList();
        
        var dates = dates1.Intersect(dates2).ToList();

        var resultCanles1 = candles1.Where(x => dates.Contains(x.Date)).OrderBy(x => x.Date).ToList();
        var resultCanles2 = candles2.Where(x => dates.Contains(x.Date)).OrderBy(x => x.Date).ToList();
        
        return (resultCanles1, resultCanles2);
    }
}