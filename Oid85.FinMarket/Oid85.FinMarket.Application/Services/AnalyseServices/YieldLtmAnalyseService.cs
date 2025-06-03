using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services.AnalyseServices;

public class YieldLtmAnalyseService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IAnalyseResultRepository analyseResultRepository)
{
    public async Task YieldLtmAnalyseAsync(Guid instrumentId)
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
            
            foreach (var candle in candles)
            {
                var yearAgoCandleDate = candle.Date.AddYears(-1);
                var currentCandleDate = candle.Date;
                
                var candlesForAnalyse = candles
                    .Where(x => 
                        x.Date >= yearAgoCandleDate && 
                        x.Date <= currentCandleDate)
                    .OrderBy(x => x.Date)
                    .ToList();
                
                var (resultString, resultNumber) = GetResult(candlesForAnalyse);
                
                results.Add(new AnalyseResult
                {
                    Date = currentCandleDate,
                    InstrumentId = instrumentId,
                    ResultString = resultString,
                    ResultNumber = resultNumber,
                    AnalyseType = KnownAnalyseTypes.YieldLtm
                });
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
        double firstPrice = candles.First().Close;
        double lastPrice = candles.Last().Close;
        double difference = lastPrice - firstPrice;
        double yield = difference / firstPrice;
        double yieldPrc = yield * 100.0;
            
        return (yieldPrc.ToString("N1"), yieldPrc);
    }
}