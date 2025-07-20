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
                var normValues1 = GetNormValues(candles[tickers[i]].Select(x => x.Close).ToList());
                var normValues2 = GetNormValues(candles[tickers[j]].Select(x => x.Close).ToList());
                double correlation = normValues1.Correlation(normValues2);

                await correlationRepository.AddAsync(
                    new Correlation
                    {
                        Ticker1 = tickers[i],
                        Ticker2 = tickers[j],
                        Value = correlation
                    });
            }
        }
    }

    private List<double> GetNormValues(List<double> values)
    {
        var delts = new List<double>();
        for (int i = 1; i < values.Count; i++) delts.Add(values[i] - values[i - 1]);
        var ln = delts.Select(x => Math.Log(x)).ToList();
        double average = ln.Average();
        double stdDev = ln.StdDev();
        var normValues = ln.Select(x => (x - average) / stdDev).ToList();
        
        return normValues;
    }
}