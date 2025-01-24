﻿using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Skender.Stock.Indicators;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Logging.Services;
using Candle = Oid85.FinMarket.Domain.Models.Candle;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class AnalyseService(
    ILogService logService,
    IShareRepository shareRepository,
    IBondRepository bondRepository,
    IFutureRepository futureRepository,
    ICurrencyRepository currencyRepository,
    IIndexRepository indexRepository,
    ICandleRepository candleRepository,
    IAnalyseResultRepository analyseResultRepository)
    : IAnalyseService
{
    /// <inheritdoc />
    public async Task<bool> AnalyseSharesAsync()
    {
        var instruments = await shareRepository.GetWatchListAsync();
            
        for (int i = 0; i < instruments.Count; i++)
        {       
            // Вызов методов анализа
            await SupertrendAnalyseAsync(instruments[i].InstrumentId);
            await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
            await CandleVolumeAnalyseAsync(instruments[i].InstrumentId);
            await RsiAnalyseAsync(instruments[i].InstrumentId);
            await YieldLtmAnalyseAsync(instruments[i].InstrumentId);

            double percent = ((i + 1) / (double) instruments.Count) * 100;

            await logService.LogTrace($"Анализ акций, {i + 1} из {instruments.Count} ({percent:N2} %)");
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> AnalyseBondsAsync()
    {
        var instruments = await bondRepository.GetWatchListAsync();
            
        for (int i = 0; i < instruments.Count; i++)
        {                
            // Вызов методов анализа
            await SupertrendAnalyseAsync(instruments[i].InstrumentId);
            await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
            await CandleVolumeAnalyseAsync(instruments[i].InstrumentId);
            await RsiAnalyseAsync(instruments[i].InstrumentId);
            await YieldLtmAnalyseAsync(instruments[i].InstrumentId);
            
            double percent = ((i + 1) / (double) instruments.Count) * 100;

            await logService.LogTrace($"Анализ облигаций, {i + 1} из {instruments.Count} ({percent:N2} %)");
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> AnalyseCurrenciesAsync()
    {
        var instruments = await currencyRepository.GetWatchListAsync();
            
        for (int i = 0; i < instruments.Count; i++)
        {                
            // Вызов методов анализа
            await SupertrendAnalyseAsync(instruments[i].InstrumentId);
            await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
            await CandleVolumeAnalyseAsync(instruments[i].InstrumentId);
            await RsiAnalyseAsync(instruments[i].InstrumentId);
            await YieldLtmAnalyseAsync(instruments[i].InstrumentId);
            
            double percent = ((i + 1) / (double) instruments.Count) * 100;

            await logService.LogTrace($"Анализ валют, {i + 1} из {instruments.Count} ({percent:N2} %)");
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> AnalyseFuturesAsync()
    {
        var instruments = await futureRepository.GetWatchListAsync();
            
        for (int i = 0; i < instruments.Count; i++)
        {                
            // Вызов методов анализа
            await SupertrendAnalyseAsync(instruments[i].InstrumentId);
            await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
            await CandleVolumeAnalyseAsync(instruments[i].InstrumentId);
            await RsiAnalyseAsync(instruments[i].InstrumentId);
            await YieldLtmAnalyseAsync(instruments[i].InstrumentId);

            double percent = ((i + 1) / (double) instruments.Count) * 100;

            await logService.LogTrace($"Анализ фьючерсов, {i + 1} из {instruments.Count} ({percent:N2} %)");
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> AnalyseIndexesAsync()
    {
        var instruments = await indexRepository.GetWatchListAsync();
            
        for (int i = 0; i < instruments.Count; i++)
        {                
            // Вызов методов анализа
            await SupertrendAnalyseAsync(instruments[i].InstrumentId);
            await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
            await CandleVolumeAnalyseAsync(instruments[i].InstrumentId);
            await RsiAnalyseAsync(instruments[i].InstrumentId);
            await YieldLtmAnalyseAsync(instruments[i].InstrumentId);

            double percent = ((i + 1) / (double) instruments.Count) * 100;

            await logService.LogTrace($"Анализ индексов, {i + 1} из {instruments.Count} ({percent:N2} %)");
        }

        return true;
    }
    
    private async Task SupertrendAnalyseAsync(Guid instrumentId)
    {
        try
        {
            var candles = (await candleRepository.GetAsync(instrumentId))
                .Where(x => x.IsComplete)
                .ToList();

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
            
            await logService.LogTrace($"Анализ 'Supertrend' выполнен. InstrumentId = '{instrumentId}'");
        }

        catch (Exception exception)
        {
            await logService.LogError($"Не удалось прочитать данные из БД finmarket. {exception}");
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
            var candles = (await candleRepository.GetAsync(instrumentId))
                .Where(x => x.IsComplete)
                .ToList();

            var results = new List<AnalyseResult>();

            for (int i = 0; i < candles.Count; i++)
            {
                var result = new AnalyseResult();

                if (i < 2)
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
            
            await logService.LogTrace($"Анализ 'CandleSequence' выполнен. InstrumentId = '{instrumentId}'");
        }

        catch (Exception exception)
        {
            await logService.LogError($"Не удалось прочитать данные из БД finmarket. {exception}");
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
            var candles = (await candleRepository.GetAsync(instrumentId))
                .Where(x => x.IsComplete)
                .ToList();

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
            
            await logService.LogTrace($"Анализ 'CandleVolume' выполнен. InstrumentId = '{instrumentId}'");
        }

        catch (Exception exception)
        {
            await logService.LogError($"Не удалось прочитать данные из БД finmarket. {exception}");
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
            var candles = (await candleRepository.GetAsync(instrumentId))
                .Where(x => x.IsComplete)
                .ToList();

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
            
            await logService.LogTrace($"Анализ 'Rsi' выполнен. InstrumentId = '{instrumentId}'");
        }

        catch (Exception exception)
        {
            await logService.LogError($"Не удалось прочитать данные из БД finmarket. {exception}");
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
            var candles = (await candleRepository.GetAsync(instrumentId))
                .Where(x => x.IsComplete)
                .ToList();

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
            
            await logService.LogTrace($"Анализ 'YieldLtm' выполнен. InstrumentId = '{instrumentId}'");
        }

        catch (Exception exception)
        {
            await logService.LogError($"Не удалось прочитать данные из БД finmarket. {exception}");
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
}