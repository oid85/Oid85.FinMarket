using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services.AnalyseServices;

public class CandleSequenceAnalyseService(
    ILogger logger,
    ICandleRepository candleRepository,
    IAnalyseResultRepository analyseResultRepository)
{
    public async Task CandleSequenceAnalyseAsync(Guid instrumentId)
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
            
            var results = new List<AnalyseResult>();

            for (int i = 0; i < candles.Count; i++)
            {
                var result = new AnalyseResult();

                if (i < 3)
                {
                    result.Date = candles[i].Date;
                    result.InstrumentId = instrumentId;
                    result.ResultString = string.Empty;
                    result.ResultNumber = 0.0;
                    result.AnalyseType = KnownAnalyseTypes.CandleSequence;
                }

                else
                {
                    var candlesForAnalyse = new List<Candle>()
                    {
                        candles[i - 2],
                        candles[i - 1],
                        candles[i]
                    };

                    (string resultString, double resultNumber) = GetResult(candlesForAnalyse);
                    
                    result.Date = candles[i].Date;
                    result.InstrumentId = instrumentId;
                    result.ResultString = resultString;
                    result.ResultNumber = resultNumber;
                    result.AnalyseType = KnownAnalyseTypes.CandleSequence;
                }                    

                results.Add(result);
            }

            await analyseResultRepository.AddAsync(results);
        }

        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка при выполнении метода. {instrumentId}", instrumentId);
        }
    }
    
    (string, double)  GetResult(List<Candle> candles)
    {
        // Свечи белые
        if (candles.All(x => x.Close > x.Open))
            return (KnownCandleSequences.White, 1.0);

        // Свечи черные
        if (candles.All(x => x.Close < x.Open))
            return (KnownCandleSequences.Black, -1.0);

        return (string.Empty, 0.0);
    }
}