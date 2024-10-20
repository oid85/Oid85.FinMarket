using Npgsql;
using Oid85.FinMarket.Application.Constants;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Settings;
using Oid85.FinMarket.External.Storage;
using Skender.Stock.Indicators;
using System.Text.Json;
using Candle = Oid85.FinMarket.Domain.Models.Candle;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.Application.Services
{
    /// <inheritdoc />
    public class AnalyseService : IAnalyseService
    {
        private readonly ILogger _logger;
        private readonly ISettingsService _settingsService;
        private readonly IStorageService _storageService;
        private readonly ICatalogService _catalogService;

        public AnalyseService(
            ILogger logger,
            ISettingsService settingsService,
            IStorageService storageService,
            ICatalogService catalogService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        }

        /// <inheritdoc />
        public async Task AnalyseStocksAsync()
        {
            var stocks = await _catalogService
                .GetActiveFinInstrumentsAsync(KnownFinInstrumentTypes.Stocks);

            for (int i = 0; i < stocks.Count; i++)
            {                
                await SupertrendAnalyseAsync(stocks[i], KnownTimeframes.Daily);
                await CandleSequenceAnalyseAsync(stocks[i], KnownTimeframes.Daily);
                await CandleVolumeAnalyseAsync(stocks[i], KnownTimeframes.Daily);
                await RsiAnalyseAsync(stocks[i], KnownTimeframes.Daily);

                double percent = ((i + 1) / (double) stocks.Count) * 100;

                _logger.Trace($"Analyse '{stocks[i].Ticker}'. {i + 1} of {stocks.Count}. {percent:N2} % completed");
            }
        }

        /// <inheritdoc />
        public async Task<List<AnalyseResult>> SupertrendAnalyseAsync(
            FinInstrument stock, string timeframe)
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
                string tableName = $"{stock.Ticker}_{timeframe}".ToLower();
                var candles = await _storageService.GetCandlesAsync(tableName);

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
                        Ticker = stock.Ticker,
                        Timeframe = timeframe,
                        Result = GetResult(x),
                        Data = JsonSerializer.Serialize(x)
                    })
                    .ToList();

                await _storageService.SaveAnalyseResultsAsync(
                    $"{stock.Ticker}_{KnownAnalyseTypes.Supertrend.Replace(" ", "_")}_{timeframe}".ToLower(), 
                    results);

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
            FinInstrument stock, string timeframe)
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
                string tableName = $"{stock.Ticker}_{timeframe}".ToLower();
                var candles = await _storageService.GetCandlesAsync(tableName);

                var results = new List<AnalyseResult>();

                for (int i = 0; i < candles.Count; i++)
                {
                    var result = new AnalyseResult();

                    if (i < 2)
                    {
                        result.Date = candles[i].Date;
                        result.Ticker = stock.Ticker;
                        result.Timeframe = timeframe;
                        result.Result = string.Empty;
                        result.Data = JsonSerializer.Serialize(candles[i]);
                    }

                    else
                    {
                        var candlesForAnalyse = new List<Candle>()
                        {
                            candles[i - 1],
                            candles[i]
                        };

                        result.Date = candles[i].Date;
                        result.Ticker = stock.Ticker;
                        result.Timeframe = timeframe;
                        result.Result = GetResult(candlesForAnalyse);
                        result.Data = JsonSerializer.Serialize(candles[i]);
                    }                    

                    results.Add(result);
                }

                await _storageService.SaveAnalyseResultsAsync(
                    $"{stock.Ticker}_{KnownAnalyseTypes.CandleSequence.Replace(" ", "_")}_{timeframe}".ToLower(),
                    results);

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
            FinInstrument stock, string timeframe)
        {
            string GetResult(List<Candle> candles)
            {
                if (candles == null)
                    return string.Empty;

                // Объем растет
                if (candles[0].Volume < candles[1].Volume && 
                    candles[1].Volume < candles[2].Volume)
                    return KnownVolumeDirections.Up;

                // Объем падает
                if (candles[0].Volume < candles[1].Volume &&
                    candles[1].Volume < candles[2].Volume)
                    return KnownVolumeDirections.Down;

                return string.Empty;
            }

            try
            {
                string tableName = $"{stock.Ticker}_{timeframe}".ToLower();
                var candles = await _storageService.GetCandlesAsync(tableName);

                var results = new List<AnalyseResult>();

                for (int i = 0; i < candles.Count; i++)
                {
                    var result = new AnalyseResult();

                    if (i < 3)
                    {
                        result.Date = candles[i].Date;
                        result.Ticker = stock.Ticker;
                        result.Timeframe = timeframe;
                        result.Result = string.Empty;
                        result.Data = JsonSerializer.Serialize(candles[i]);
                    }

                    else
                    {
                        var candlesForAnalyse = new List<Candle>()
                        {
                            candles[i - 2], 
                            candles[i - 1],
                            candles[i]
                        };

                        result.Date = candles[i].Date;
                        result.Ticker = stock.Ticker;
                        result.Timeframe = timeframe;
                        result.Result = GetResult(candlesForAnalyse);
                        result.Data = JsonSerializer.Serialize(candles[i]);
                    }

                    results.Add(result);
                }

                await _storageService.SaveAnalyseResultsAsync(
                    $"{stock.Ticker}_{KnownAnalyseTypes.CandleVolume.Replace(" ", "_")}_{timeframe}".ToLower(),
                    results);

                return results;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
            }
        }

        /// <inheritdoc />
        public async Task<List<AnalyseResult>> RsiAnalyseAsync(
            FinInstrument stock, string timeframe)
        {
            string GetResult(RsiResult result)
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

            try
            {
                string tableName = $"{stock.Ticker}_{timeframe}".ToLower();
                var candles = await _storageService.GetCandlesAsync(tableName);

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
                        Ticker = stock.Ticker,
                        Timeframe = timeframe,
                        Result = GetResult(x),
                        Data = JsonSerializer.Serialize(x)
                    })
                    .ToList();

                await _storageService.SaveAnalyseResultsAsync(
                    $"{stock.Ticker}_{KnownAnalyseTypes.Rsi.Replace(" ", "_")}_{timeframe}".ToLower(),
                    results);

                return results;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
            }
        }

        /// <summary>
        /// Получить соединение с БД
        /// </summary>
        private async Task<NpgsqlConnection> GetPostgresConnectionAsync()
        {
            try
            {
                var connectionString = await _settingsService
                    .GetStringValueAsync(KnownSettingsKeys.Postgres_ConnectionString);

                var connection = new NpgsqlConnection(connectionString);

                return connection;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось установить соединение с БД finmarket. {exception}");
                throw new Exception($"Не удалось установить соединение с БД finmarket. {exception}");
            }
        }
    }
}
