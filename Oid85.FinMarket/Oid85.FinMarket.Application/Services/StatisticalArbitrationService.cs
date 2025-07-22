using System.Diagnostics;
using System.Text.Json;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Common.MathExtensions;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Domain.Models.StatisticalArbitration;

namespace Oid85.FinMarket.Application.Services;

public class StatisticalArbitrationService(
    ITickerListUtilService tickerListUtilService,
    IDailyCandleRepository dailyCandleRepository,
    ICorrelationRepository correlationRepository) 
    : IStatisticalArbitrationService
{
    /// <inheritdoc />
    public async Task CalculateCorrelationAsync()
    {
        var shares = await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.StatisticalArbitrationShares);
        var futures = (await tickerListUtilService.GetFuturesByTickerListAsync(KnownTickerLists.StatisticalArbitrationFutures))
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
                    
                }
            }
        }
    }

    private (List<DailyCandle> Candles1, List<DailyCandle> Candles2) SyncCandles(List<DailyCandle> candles1, List<DailyCandle> candles2)
    {
        var dates1 = candles1.Select(x => x.Date).ToList();
        var dates2 = candles2.Select(x => x.Date).ToList();
        
        var dates = dates1.Intersect(dates2).ToList();

        var resultCanles1 = candles1.Where(x => dates.Contains(x.Date)).OrderBy(x => x.Date).ToList();
        var resultCanles2 = candles2.Where(x => dates.Contains(x.Date)).OrderBy(x => x.Date).ToList();
        
        return (resultCanles1, resultCanles2);
    }
}