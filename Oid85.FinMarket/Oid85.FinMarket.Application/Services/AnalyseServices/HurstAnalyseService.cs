using NLog;
using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Domain.Models.Algo;
using Skender.Stock.Indicators;

namespace Oid85.FinMarket.Application.Services.AnalyseServices;

public class HurstAnalyseService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IAnalyseResultRepository analyseResultRepository,
    IIndicatorFactory indicatorFactory)
{
    public async Task HurstAnalyseAsync(Guid instrumentId)
    {
        try
        {
            var candles = (await dailyCandleRepository.GetLastYearAsync(instrumentId))
                .Where(x => x.IsComplete)
                .Select(x => new Candle
                {
                    Open = x.Open,
                    Close = x.Close,
                    High = x.High,
                    Low = x.Low,
                    Volume = x.Volume,
                    DateTime = x.Date.ToDateTime(TimeOnly.MinValue)
                })
                .ToList();

            if (candles is [])
            {
                logger.Warn($"По инструменту '{instrumentId}' нет ни одной свечи");
                return;
            }
            
            const int period = 50;

            var hurstResults = indicatorFactory.Hurst(candles, period);

            var results = new List<AnalyseResult>();

            for (int i = 0; i < hurstResults.Count; i++)
            {
                var (resultString, resultNumber) = GetResult(hurstResults[i]);
                
                results.Add(new AnalyseResult
                {
                    Date = DateOnly.FromDateTime(candles[i].DateTime),
                    InstrumentId = instrumentId,
                    ResultString = resultString,
                    ResultNumber = resultNumber,
                    AnalyseType = KnownAnalyseTypes.Hurst
                });
            }

            await analyseResultRepository.AddAsync(results);
        }

        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка при выполнении метода. {instrumentId}", instrumentId);
        }
    }

    private static (string, double) GetResult(double result) =>
        (result.ToString("N2"), result);
}