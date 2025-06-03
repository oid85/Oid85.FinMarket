using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Skender.Stock.Indicators;

namespace Oid85.FinMarket.Application.Services.AnalyseServices;

public class DonchianAnalyseService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IAnalyseResultRepository analyseResultRepository)
{
    public async Task DonchianAnalyseAsync(Guid instrumentId)
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
            
            const int lookbackPeriods = 50;

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

            var donchianResults = quotes.GetDonchian(lookbackPeriods).ToList();
            donchianResults = ListHelper.ShiftRight(donchianResults)!;
            
            var analyseResults = new List<AnalyseResult>();
            
            foreach (var donchianResult in donchianResults)
            {
                if (donchianResult is null)
                    continue;
                
                var candle = candles.Find(x => x.Date == DateOnly.FromDateTime(donchianResult.Date));
				
                if (candle is null)
                    continue;
                
                var price = candle.Close;
                
                var (resultString, resultNumber) = GetResult(donchianResult, Convert.ToDecimal(price));
                
                var analyseResult = new AnalyseResult()
                {
                    Date = DateOnly.FromDateTime(donchianResult.Date),
                    InstrumentId = instrumentId,
                    ResultString = resultString,
                    ResultNumber = resultNumber,
                    AnalyseType = KnownAnalyseTypes.Donchian
                };
                
                analyseResults.Add(analyseResult);
            }

            await analyseResultRepository.AddAsync(analyseResults);
        }

        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка при выполнении метода. {instrumentId}", instrumentId);
        }
    }
    
    (string, double) GetResult(DonchianResult result, decimal price)
    {
        if (result is {UpperBand: not null, LowerBand: not null})
            if (price > result.UpperBand)
                return (KnownTrendDirections.Up, 1.0);

        if (result is {UpperBand: not null, LowerBand: not null})
            if (price < result.LowerBand)
                return (KnownTrendDirections.Down, -1.0);

        return (string.Empty, 0.0);
    }
}