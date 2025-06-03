using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services.AnalyseServices;

public class CandleVolumeAnalyseService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IAnalyseResultRepository analyseResultRepository)
{
    public async Task CandleVolumeAnalyseAsync(Guid instrumentId)
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
            
            var results = new List<AnalyseResult>();

            for (int i = 0; i < candles.Count; i++)
            {
                var result = new AnalyseResult();

                if (i < 10)
                {
                    result.Date = candles[i].Date;
                    result.InstrumentId = instrumentId;
                    result.ResultString = string.Empty;
                    result.ResultNumber = 0.0;
                    result.AnalyseType = KnownAnalyseTypes.CandleVolume;
                }

                else
                {
                    var candlesForAnalyse = new List<DailyCandle>()
                    {
                        candles[i - 9],
                        candles[i - 8],
                        candles[i - 7],
                        candles[i - 6],
                        candles[i - 5],
                        candles[i - 4],
                        candles[i - 3],
                        candles[i - 2],
                        candles[i - 1],
                        candles[i]
                    };

                    (string resultString, double resultNumber) = GetResult(candlesForAnalyse);
                    
                    result.Date = candles[i].Date;
                    result.InstrumentId = instrumentId;
                    result.ResultString = resultString;
                    result.ResultNumber = resultNumber;
                    result.AnalyseType = KnownAnalyseTypes.CandleVolume;
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
    
    (string, double) GetResult(List<DailyCandle> candles)
    {
        // Объем последней свечи выше, чем у всех предыдущих
        var lastVolume = candles.Last().Volume;
        var prevVolumes = candles
            .Select(x => x.Volume)
            .Take(candles.Count - 1);

        bool extremalVolume = lastVolume > prevVolumes.Max();

        // Объем растет в течении 3 свечей
        bool grossVolume = 
            candles[^2].Volume > candles[^3].Volume &&
            candles[^1].Volume > candles[^2].Volume;
            
        if (extremalVolume && grossVolume)
            return (KnownVolumeDirections.Up, 1.0);
        
        return (string.Empty, 0.0);
    }
}