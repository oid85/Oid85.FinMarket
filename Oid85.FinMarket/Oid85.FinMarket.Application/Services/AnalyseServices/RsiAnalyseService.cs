using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Skender.Stock.Indicators;

namespace Oid85.FinMarket.Application.Services.AnalyseServices;

public class RsiAnalyseService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IAnalyseResultRepository analyseResultRepository)
{
    public async Task RsiAnalyseAsync(Guid instrumentId)
    {
        try
        {
            var candles = (await dailyCandleRepository.GetLastYearAsync(instrumentId))
                .Where(x => x.IsComplete)
                .ToList();

            if (candles is [])
            {
                logger.Warn($"По инструменту '{instrumentId}' нет ни одной свечи");
                return;
            }
            
            int lookbackPeriods = 14;

            var quotes = candles
                .Select(x => new Quote()
                {
                    Open = Convert.ToDecimal(x.Open),
                    Close = Convert.ToDecimal(x.Close),
                    High = Convert.ToDecimal(x.High),
                    Low = Convert.ToDecimal(x.Low),
                    Date = x.Date.ToDateTime(TimeOnly.MinValue)
                })
                .ToList();

            var rsiResults = quotes.GetRsi(lookbackPeriods);

            var results = rsiResults
                .Select(x =>
                {
                    (string resultString, double resultNumber) = GetResult(x);
                    
                    return new AnalyseResult()
                    {
                        Date = DateOnly.FromDateTime(x.Date),
                        InstrumentId = instrumentId,
                        ResultString = resultString,
                        ResultNumber = resultNumber,
                        AnalyseType = KnownAnalyseTypes.Rsi
                    };
                })
                .ToList();

            await analyseResultRepository.AddAsync(results);
        }

        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка при выполнении метода. {instrumentId}", instrumentId);
        }
    }
    
    (string, double) GetResult(RsiResult result)
    {
        const double upLimit = 60.0;
        const double downLimit = 40.0;

        if (result.Rsi == null)
            return (string.Empty, 0.0);

        if (result.Rsi >= upLimit)
            return (KnownRsiInterpretations.OverBought, -1.0);

        if (result.Rsi <= downLimit)
            return (KnownRsiInterpretations.OverSold, 1.0);

        return (string.Empty, 0.0);
    }
}