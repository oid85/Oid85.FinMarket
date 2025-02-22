using NLog;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Skender.Stock.Indicators;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Candle = Oid85.FinMarket.Domain.Models.Candle;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class AnalyseService(
    ILogger logger,
    IInstrumentService instrumentService,
    ICandleRepository candleRepository,
    IAnalyseResultRepository analyseResultRepository)
    : IAnalyseService
{
    /// <inheritdoc />
    public async Task<bool> AnalyseSharesAsync()
    {
        try
        {
            var instruments = await instrumentService.GetSharesInWatchlist();

            if (instruments is [])
            {
                logger.Warn("В БД нет ни одной акции");
                return true;
            }

            for (int i = 0; i < instruments.Count; i++)
            {       
                // Вызов методов анализа
                await SupertrendAnalyseAsync(instruments[i].InstrumentId);
                await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
                await CandleVolumeAnalyseAsync(instruments[i].InstrumentId);
                await RsiAnalyseAsync(instruments[i].InstrumentId);
                await YieldLtmAnalyseAsync(instruments[i].InstrumentId);
                await DrawdownFromMaximumAnalyseAsync(instruments[i].InstrumentId);

                double percent = ((i + 1) / (double) instruments.Count) * 100;

                logger.Trace($"Анализ акций, {i + 1} из {instruments.Count} ({percent:N2} %)");
            }

            return true;
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> AnalyseBondsAsync()
    {
        try
        {
            var instruments = await instrumentService.GetBondsInWatchlist();
            
            if (instruments is [])
            {
                logger.Warn("В БД нет ни одной облигации");
                return true;
            }
            
            for (int i = 0; i < instruments.Count; i++)
            {                
                // Вызов методов анализа
                await SupertrendAnalyseAsync(instruments[i].InstrumentId);
                await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
                await CandleVolumeAnalyseAsync(instruments[i].InstrumentId);
            
                double percent = ((i + 1) / (double) instruments.Count) * 100;

                logger.Trace($"Анализ облигаций, {i + 1} из {instruments.Count} ({percent:N2} %)");
            }

            return true;
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> AnalyseCurrenciesAsync()
    {
        try
        {
            var instruments = await instrumentService.GetCurrenciesInWatchlist();
            
            if (instruments is [])
            {
                logger.Warn("В БД нет ни одной валюты");
                return true;
            }
            
            for (int i = 0; i < instruments.Count; i++)
            {                
                // Вызов методов анализа
                await SupertrendAnalyseAsync(instruments[i].InstrumentId);
                await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
                await RsiAnalyseAsync(instruments[i].InstrumentId);
                await YieldLtmAnalyseAsync(instruments[i].InstrumentId);
                await DrawdownFromMaximumAnalyseAsync(instruments[i].InstrumentId);
            
                double percent = ((i + 1) / (double) instruments.Count) * 100;

                logger.Trace($"Анализ валют, {i + 1} из {instruments.Count} ({percent:N2} %)");
            }

            return true;
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> AnalyseFuturesAsync()
    {
        try
        {
            var instruments = await instrumentService.GetFuturesInWatchlist();
            
            if (instruments is [])
            {
                logger.Warn("В БД нет ни одного фьючерса");
                return true;
            }
            
            for (int i = 0; i < instruments.Count; i++)
            {                
                // Вызов методов анализа
                await SupertrendAnalyseAsync(instruments[i].InstrumentId);
                await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
                await CandleVolumeAnalyseAsync(instruments[i].InstrumentId);
                await RsiAnalyseAsync(instruments[i].InstrumentId);
                await YieldLtmAnalyseAsync(instruments[i].InstrumentId);

                double percent = ((i + 1) / (double) instruments.Count) * 100;

                logger.Trace($"Анализ фьючерсов, {i + 1} из {instruments.Count} ({percent:N2} %)");
            }

            return true;
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> AnalyseIndexesAsync()
    {
        try
        {
            var instruments = await instrumentService.GetFinIndexesInWatchlist();
            
            if (instruments is [])
            {
                logger.Warn("В БД нет ни одного индекса");
                return true;
            }
            
            for (int i = 0; i < instruments.Count; i++)
            {                
                // Вызов методов анализа
                await SupertrendAnalyseAsync(instruments[i].InstrumentId);
                await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
                await RsiAnalyseAsync(instruments[i].InstrumentId);
                await YieldLtmAnalyseAsync(instruments[i].InstrumentId);
                await DrawdownFromMaximumAnalyseAsync(instruments[i].InstrumentId);

                double percent = ((i + 1) / (double) instruments.Count) * 100;

                logger.Trace($"Анализ индексов, {i + 1} из {instruments.Count} ({percent:N2} %)");
            }

            return true;
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
            return false;
        }
    }
    
    private async Task SupertrendAnalyseAsync(Guid instrumentId)
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
                    Date = x.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime()
                })
                .ToList();

            var superTrendResults = quotes.GetSuperTrend(lookbackPeriods);

            var results = superTrendResults
                .Select(x =>
                {
                    var (resultString, resultNumber) = GetResult(x);
                    
                    return new AnalyseResult()
                    {
                        Date = DateOnly.FromDateTime(x.Date),
                        InstrumentId = instrumentId,
                        ResultString = resultString,
                        ResultNumber = resultNumber,
                        AnalyseType = KnownAnalyseTypes.Supertrend
                    };
                })
                .ToList();

            await analyseResultRepository.AddAsync(results);
            
            logger.Trace($"Анализ 'Supertrend' выполнен. InstrumentId = '{instrumentId}'");
        }

        catch (Exception exception)
        {
            logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
        }

        return;

        (string, double) GetResult(SuperTrendResult result)
        {
            if (result is {SuperTrend: null})
                return (string.Empty, 0.0);

            if (result is {UpperBand: null, LowerBand: not null})
                return (KnownTrendDirections.Up, 1.0);

            if (result is {UpperBand: not null, LowerBand: null})
                return (KnownTrendDirections.Down, -1.0);

            return (string.Empty, 0.0);
        }
    }
    
    private async Task CandleSequenceAnalyseAsync(Guid instrumentId)
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
            
            logger.Trace($"Анализ 'CandleSequence' выполнен. InstrumentId = '{instrumentId}'");
        }

        catch (Exception exception)
        {
            logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
        }

        return;

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
    
    private async Task CandleVolumeAnalyseAsync(Guid instrumentId)
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
                    var candlesForAnalyse = new List<Candle>()
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
            
            logger.Trace($"Анализ 'CandleVolume' выполнен. InstrumentId = '{instrumentId}'");
        }

        catch (Exception exception)
        {
            logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
        }

        return;

        (string, double) GetResult(List<Candle> candles)
        {
            var lastVolume = candles.Last().Volume;
            var prevVolumes = candles
                .Select(x => x.Volume)
                .Take(candles.Count - 1);

            // Объем последней свечи выше, чем у всех предыдущих
            if (lastVolume > prevVolumes.Max())
                return (KnownVolumeDirections.Up, 1.0);

            return (string.Empty, 0.0);
        }
    }
    
    private async Task RsiAnalyseAsync(Guid instrumentId)
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
            
            int lookbackPeriods = 14;

            var quotes = candles
                .Select(x => new Quote()
                {
                    Open = Convert.ToDecimal(x.Open),
                    Close = Convert.ToDecimal(x.Close),
                    High = Convert.ToDecimal(x.High),
                    Low = Convert.ToDecimal(x.Low),
                    Date = x.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime()
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
            
            logger.Trace($"Анализ 'Rsi' выполнен. InstrumentId = '{instrumentId}'");
        }

        catch (Exception exception)
        {
            logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
        }

        return;

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
    
    private async Task YieldLtmAnalyseAsync(Guid instrumentId)
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
            
            logger.Trace($"Анализ 'YieldLtm' выполнен. InstrumentId = '{instrumentId}'");
        }

        catch (Exception exception)
        {
            logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
        }

        return;

        (string, double) GetResult(List<Candle> candles)
        {
            double firstPrice = candles.First().Close;
            double lastPrice = candles.Last().Close;
            double difference = lastPrice - firstPrice;
            double yield = difference / firstPrice;
            double yieldPrc = yield * 100.0;
            
            return (yieldPrc.ToString("N2"), yieldPrc);
        }
    }
    
        private async Task DrawdownFromMaximumAnalyseAsync(Guid instrumentId)
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
            
            logger.Trace($"Анализ 'DrawdownFromMaximum' выполнен. InstrumentId = '{instrumentId}'");
        }

        catch (Exception exception)
        {
            logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
        }

        return;

        (string, double) GetResult(List<Candle> candles)
        {
            double maxPrice = candles.Max(x => x.High);
            double lastPrice = candles.Last().Close;
            
            if (lastPrice > maxPrice)
                return ("0.0", 0.0);
            
            double difference = maxPrice - lastPrice;
            double drawdown = difference / maxPrice;
            double drawdownPrc = drawdown * 100.0;
            
            return (drawdownPrc.ToString("N2"), drawdownPrc);
        }
    }
}