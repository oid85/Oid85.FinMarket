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
                    var prices1 = candles[tickers[i]].Select(x => x.Close).ToList();
                    var prices2 = candles[tickers[j]].Select(x => x.Close).ToList();
                    
                    if (prices1.Count != prices2.Count) continue;
                    
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

                    if (double.IsNaN(correlation)) continue;
                    
                    await correlationRepository.AddAsync(new Correlation
                    {
                        Ticker1 = tickers[i], 
                        Ticker2 = tickers[j], 
                        Value = correlation
                    });
                }
                
                catch (Exception exception)
                {

                }
            }
        }
    }
}