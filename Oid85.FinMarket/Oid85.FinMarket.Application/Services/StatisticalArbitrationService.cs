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
                    
                    double correlation = prices1.Correlation(prices2);

                    if (!double.IsNaN(correlation))
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