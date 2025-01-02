using Oid85.FinMarket.Common.KnownConstants;
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
            // Вызов методом анализа
            await SupertrendAnalyseAsync(instruments[i].InstrumentId);
            await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
            await CandleVolumeAnalyseAsync(instruments[i].InstrumentId);
            await RsiAnalyseAsync(instruments[i].InstrumentId);
            await YieldLtmAnalyseAsync(instruments[i].InstrumentId);

            double percent = ((i + 1) / (double) instruments.Count) * 100;

            await logService.LogTrace($"Анализ акций, {i + 1} из {instruments.Count}. {percent:N2} % закончено");
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> AnalyseBondsAsync()
    {
        var instruments = await bondRepository.GetWatchListAsync();
            
        for (int i = 0; i < instruments.Count; i++)
        {                
            // Вызов методом анализа
            await SupertrendAnalyseAsync(instruments[i].InstrumentId);
            await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
            await CandleVolumeAnalyseAsync(instruments[i].InstrumentId);
            await RsiAnalyseAsync(instruments[i].InstrumentId);
            await YieldLtmAnalyseAsync(instruments[i].InstrumentId);
            
            double percent = ((i + 1) / (double) instruments.Count) * 100;

            await logService.LogTrace($"Анализ облигаций, {i + 1} из {instruments.Count}. {percent:N2} % закончено");
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> AnalyseCurrenciesAsync()
    {
        var instruments = await currencyRepository.GetWatchListAsync();
            
        for (int i = 0; i < instruments.Count; i++)
        {                
            // Вызов методом анализа
            await SupertrendAnalyseAsync(instruments[i].InstrumentId);
            await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
            await CandleVolumeAnalyseAsync(instruments[i].InstrumentId);
            await RsiAnalyseAsync(instruments[i].InstrumentId);
            await YieldLtmAnalyseAsync(instruments[i].InstrumentId);
            
            double percent = ((i + 1) / (double) instruments.Count) * 100;

            await logService.LogTrace($"Анализ валют, {i + 1} из {instruments.Count}. {percent:N2} % закончено");
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> AnalyseFuturesAsync()
    {
        var instruments = await futureRepository.GetWatchListAsync();
            
        for (int i = 0; i < instruments.Count; i++)
        {                
            // Вызов методом анализа
            await SupertrendAnalyseAsync(instruments[i].InstrumentId);
            await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
            await CandleVolumeAnalyseAsync(instruments[i].InstrumentId);
            await RsiAnalyseAsync(instruments[i].InstrumentId);
            await YieldLtmAnalyseAsync(instruments[i].InstrumentId);

            double percent = ((i + 1) / (double) instruments.Count) * 100;

            await logService.LogTrace($"Анализ фьючерсов, {i + 1} из {instruments.Count}. {percent:N2} % закончено");
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> AnalyseIndexesAsync()
    {
        var instruments = await indexRepository.GetWatchListAsync();
            
        for (int i = 0; i < instruments.Count; i++)
        {                
            // Вызов методом анализа
            await SupertrendAnalyseAsync(instruments[i].InstrumentId);
            await CandleSequenceAnalyseAsync(instruments[i].InstrumentId);
            await CandleVolumeAnalyseAsync(instruments[i].InstrumentId);
            await RsiAnalyseAsync(instruments[i].InstrumentId);
            await YieldLtmAnalyseAsync(instruments[i].InstrumentId);

            double percent = ((i + 1) / (double) instruments.Count) * 100;

            await logService.LogTrace($"Анализ индексов, {i + 1} из {instruments.Count}. {percent:N2} % закончено");
        }

        return true;
    }
    
    private async Task SupertrendAnalyseAsync(Guid instrumentId)
    {
        string GetResult(SuperTrendResult result)
        {
            if (result.SuperTrend == null)
                return string.Empty;

            if (result.UpperBand == null && result.LowerBand != null)
                return KnownTrendDirections.Up;

            if (result.UpperBand != null && result.LowerBand == null)
                return KnownTrendDirections.Down;

            return string.Empty;
        }
            
        try
        {
            var candles = (await candleRepository.GetAsync(instrumentId))
                .Where(x => x.IsComplete)
                .ToList();

            const int lookbackPeriods = 50;
            const double multiplier = 3.0;

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

            var superTrendResults = quotes.GetSuperTrend(lookbackPeriods, multiplier);

            var results = superTrendResults
                .Select(x => new AnalyseResult()
                {
                    Date = DateOnly.FromDateTime(x.Date),
                    InstrumentId = instrumentId,
                    Result = GetResult(x)
                })
                .ToList();

            await analyseResultRepository.AddAsync(results);
        }

        catch (Exception exception)
        {
            await logService.LogError($"Не удалось прочитать данные из БД finmarket. {exception}");
            throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
        }
    }
    
    private async Task CandleSequenceAnalyseAsync(Guid instrumentId)
    {
        string GetResult(List<Candle> candles)
        {
            // Свечи белые
            if (candles.All(x => x.Close > x.Open))
                return KnownCandleSequences.White;

            // Свечи черные
            if (candles.All(x => x.Close < x.Open))
                return KnownCandleSequences.Black;

            return string.Empty;
        } 
            
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
                    result.Result = string.Empty;
                }

                else
                {
                    var candlesForAnalyse = new List<Candle>()
                    {
                        candles[i - 1],
                        candles[i]
                    };

                    result.Date = candles[i].Date;
                    result.InstrumentId = instrumentId;
                    result.Result = GetResult(candlesForAnalyse);
                }                    

                results.Add(result);
            }

            await analyseResultRepository.AddAsync(results);
        }

        catch (Exception exception)
        {
            await logService.LogError($"Не удалось прочитать данные из БД finmarket. {exception}");
            throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
        }
    }
    
    private async Task CandleVolumeAnalyseAsync(Guid instrumentId)
    {
        string GetResult(List<Candle> candles)
        {
            var lastVolume = candles.Last().Volume;
            var prevVolumes = candles
                .Select(x => x.Volume)
                .Take(candles.Count - 1);

            // Объем последней свечи выше, чем у всех предыдущих
            if (lastVolume > prevVolumes.Max())
                return KnownVolumeDirections.Up;

            return string.Empty;
        }
            
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
                    result.Result = string.Empty;
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

                    result.Date = candles[i].Date;
                    result.InstrumentId = instrumentId;
                    result.Result = GetResult(candlesForAnalyse);
                }

                results.Add(result);
            }

            await analyseResultRepository.AddAsync(results);
        }

        catch (Exception exception)
        {
            await logService.LogError($"Не удалось прочитать данные из БД finmarket. {exception}");
            throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
        }
    }
    
    private async Task RsiAnalyseAsync(Guid instrumentId)
    {
        string GetRsiResult(RsiResult result)
        {
            const double upLimit = 60.0;
            const double downLimit = 40.0;

            if (result.Rsi == null)
                return string.Empty;

            if (result.Rsi >= upLimit)
                return KnownRsiInterpretations.OverBought;

            if (result.Rsi <= downLimit)
                return KnownRsiInterpretations.OverSold;

            return string.Empty;
        }       
        
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
                .Select(x => new AnalyseResult()
                {
                    Date = DateOnly.FromDateTime(x.Date),
                    InstrumentId = instrumentId,
                    Result = GetRsiResult(x)
                })
                .ToList();

            await analyseResultRepository.AddAsync(results);
        }

        catch (Exception exception)
        {
            await logService.LogError($"Не удалось прочитать данные из БД finmarket. {exception}");
            throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
        }
    }
    
    private async Task YieldLtmAnalyseAsync(Guid instrumentId)
    {
        string GetResult(List<Candle> candles)
        {
            double firstPrice = candles.First().Close;
            double lastPrice = candles.Last().Close;
            double difference = lastPrice - firstPrice;
            double yield = difference / firstPrice;
            double yieldPrc = yield * 100.0;
            
            return yieldPrc.ToString("N2");
        }
            
        try
        {
            var candles = (await candleRepository.GetAsync(instrumentId))
                .Where(x => x.IsComplete)
                .ToList();

            var results = new List<AnalyseResult>();
            
            for (int i = 0; i < candles.Count; i++)
            {
                var yearAgoCandleDate = candles[i].Date.AddYears(-1);
                var currentCandleDate = candles[i].Date;
                
                var windowCandles = candles
                    .Where(x => 
                        x.Date >= yearAgoCandleDate && 
                        x.Date <= currentCandleDate)
                    .OrderBy(x => x.Date)
                    .ToList();
                
                results.Add(new AnalyseResult
                {
                    AnalyseType = KnownAnalyseTypes.YieldLtm,
                    Date = currentCandleDate,
                    InstrumentId = instrumentId,
                    Result = GetResult(windowCandles)
                });
            }

            await analyseResultRepository.AddAsync(results);
        }

        catch (Exception exception)
        {
            await logService.LogError($"Не удалось прочитать данные из БД finmarket. {exception}");
            throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
        }
    }
}