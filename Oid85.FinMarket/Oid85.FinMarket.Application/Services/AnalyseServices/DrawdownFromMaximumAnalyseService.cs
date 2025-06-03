using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services.AnalyseServices;

public class DrawdownFromMaximumAnalyseService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IAnalyseResultRepository analyseResultRepository)
{
    public async Task DrawdownFromMaximumAnalyseAsync(Guid instrumentId)
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
                    AnalyseType = KnownAnalyseTypes.DrawdownFromMaximum
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
        double maxPrice = candles.Max(x => x.High);
        double lastPrice = candles.Last().Close;
            
        if (lastPrice > maxPrice)
            return ("0.0", 0.0);
            
        double difference = maxPrice - lastPrice;
        double drawdown = difference / maxPrice;
        double drawdownPrc = drawdown * 100.0;
            
        return (drawdownPrc.ToString("N1"), drawdownPrc);
    }
}