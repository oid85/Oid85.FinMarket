using Oid85.FinMarket.Application.Constants;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Skender.Stock.Indicators;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Candle = Oid85.FinMarket.Domain.Models.Candle;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.Application.Services
{
    /// <inheritdoc />
    public class AnalyseService : IAnalyseService
    {
        private readonly ILogger _logger;
        private readonly IShareRepository _shareRepository;
        private readonly ICandleRepository _candleRepository;
        private readonly IAnalyseResultRepository _analyseResultRepository;

        public AnalyseService(
            ILogger logger, 
            IShareRepository shareRepository, 
            ICandleRepository candleRepository, 
            IAnalyseResultRepository analyseResultRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _shareRepository = shareRepository ?? throw new ArgumentNullException(nameof(shareRepository));
            _candleRepository = candleRepository ?? throw new ArgumentNullException(nameof(candleRepository));
            _analyseResultRepository = analyseResultRepository ?? throw new ArgumentNullException(nameof(analyseResultRepository));
        }

        /// <inheritdoc />
        public async Task AnalyseStocksAsync()
        {
            var shares = await _shareRepository.GetSharesAsync();
            var timeframe = KnownTimeframes.Daily;
            
            for (int i = 0; i < shares.Count; i++)
            {                
                await SupertrendAnalyseAsync(shares[i], timeframe);
                await CandleSequenceAnalyseAsync(shares[i], timeframe);
                await CandleVolumeAnalyseAsync(shares[i], timeframe);
                await RsiAnalyseAsync(shares[i], timeframe);

                double percent = ((i + 1) / (double) shares.Count) * 100;

                _logger.Trace($"Analyse '{shares[i].Ticker}'. {i + 1} of {shares.Count}. {percent:N2} % completed");
            }
        }
        
        /// <inheritdoc />
        public async Task<List<AnalyseResult>> SupertrendAnalyseAsync(
            Share share, string timeframe)
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
                var candles = (await _candleRepository.GetCandlesAsync(share.Ticker, timeframe))
                    .Where(x => x.IsComplete)
                    .ToList();

                int lookbackPeriods = 50;
                double multiplier = 3.0;

                var quotes = candles
                    .Select(x => new Quote()
                    {
                        Open = Convert.ToDecimal(x.Open),
                        Close = Convert.ToDecimal(x.Close),
                        High = Convert.ToDecimal(x.High),
                        Low = Convert.ToDecimal(x.Low),
                        Date = x.Date.ToUniversalTime()
                    })
                    .ToList();

                var superTrendResults = quotes.GetSuperTrend(lookbackPeriods, multiplier);

                var results = superTrendResults
                    .Select(x => new AnalyseResult()
                    {
                        Date = x.Date.ToUniversalTime(),
                        Ticker = share.Ticker,
                        Timeframe = timeframe,
                        Result = GetResult(x)
                    })
                    .ToList();

                await _analyseResultRepository.AddOrUpdateAsync(results);

                return results;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
            }
        }
        
        /// <inheritdoc />
        public async Task<List<AnalyseResult>> CandleSequenceAnalyseAsync(
            Share share, string timeframe)
        {
            string GetResult(List<Candle> candles)
            {
                if (candles == null)
                    return string.Empty;

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
                var candles = (await _candleRepository.GetCandlesAsync(share.Ticker, timeframe))
                    .Where(x => x.IsComplete)
                    .ToList();

                var results = new List<AnalyseResult>();

                for (int i = 0; i < candles.Count; i++)
                {
                    var result = new AnalyseResult();

                    if (i < 2)
                    {
                        result.Date = candles[i].Date;
                        result.Ticker = share.Ticker;
                        result.Timeframe = timeframe;
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
                        result.Ticker = share.Ticker;
                        result.Timeframe = timeframe;
                        result.Result = GetResult(candlesForAnalyse);
                    }                    

                    results.Add(result);
                }

                await _analyseResultRepository.AddOrUpdateAsync(results);

                return results;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
            }
        }
        
        /// <inheritdoc />
        public async Task<List<AnalyseResult>> CandleVolumeAnalyseAsync(
            Share share, string timeframe)
        {
            string GetResult(List<Candle> candles)
            {
                if (candles == null)
                    return string.Empty;

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
                var candles = (await _candleRepository.GetCandlesAsync(share.Ticker, timeframe))
                    .Where(x => x.IsComplete)
                    .ToList();

                var results = new List<AnalyseResult>();

                for (int i = 0; i < candles.Count; i++)
                {
                    var result = new AnalyseResult();

                    if (i < 10)
                    {
                        result.Date = candles[i].Date;
                        result.Ticker = share.Ticker;
                        result.Timeframe = timeframe;
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
                        result.Ticker = share.Ticker;
                        result.Timeframe = timeframe;
                        result.Result = GetResult(candlesForAnalyse);
                    }

                    results.Add(result);
                }

                await _analyseResultRepository.AddOrUpdateAsync(results);

                return results;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
            }
        }

        private string GetRsiResult(RsiResult result)
        {
            double upLimit = 60.0;
            double downLimit = 40.0;

            if (result.Rsi == null)
                return string.Empty;

            if (result.Rsi >= upLimit)
                return KnownRsiInterpretations.OverBought;

            if (result.Rsi <= downLimit)
                return KnownRsiInterpretations.OverSold;

            return string.Empty;
        }        
        
        /// <inheritdoc />
        public async Task<List<AnalyseResult>> RsiAnalyseAsync(
            Share share, string timeframe)
        {
            try
            {
                var candles = (await _candleRepository.GetCandlesAsync(share.Ticker, timeframe))
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
                        Date = x.Date.ToUniversalTime()
                    })
                    .ToList();

                var rsiResults = quotes.GetRsi(lookbackPeriods);

                var results = rsiResults
                    .Select(x => new AnalyseResult()
                    {
                        Date = x.Date.ToUniversalTime(),
                        Ticker = share.Ticker,
                        Timeframe = timeframe,
                        Result = GetRsiResult(x)
                    })
                    .ToList();

                await _analyseResultRepository.AddOrUpdateAsync(results);

                return results;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
            }
        }
    }
}
