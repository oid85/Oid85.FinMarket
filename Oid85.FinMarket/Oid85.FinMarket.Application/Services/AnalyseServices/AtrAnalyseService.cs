using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Skender.Stock.Indicators;

namespace Oid85.FinMarket.Application.Services.AnalyseServices;

public class AtrAnalyseService(
    ILogger logger,
    ICandleRepository candleRepository,
    IAnalyseResultRepository analyseResultRepository)
{
    public async Task AtrAnalyseAsync(Guid instrumentId)
    {
        try
        {
            var candles = (await candleRepository.GetLastYearAsync(instrumentId))
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

            var atrResults = quotes.GetAtr(lookbackPeriods);

            var results = atrResults
                .Select(x =>
                {
                    var (resultString, resultNumber) = GetResult(x);
                    
                    return new AnalyseResult()
                    {
                        Date = DateOnly.FromDateTime(x.Date),
                        InstrumentId = instrumentId,
                        ResultString = resultString,
                        ResultNumber = resultNumber,
                        AnalyseType = KnownAnalyseTypes.Atr
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
    
    (string, double) GetResult(AtrResult result) =>
        result.Atr is null 
            ? (string.Empty, 0.0) 
            : (result.Atr.Value.ToString("N2"), result.Atr.Value);
}