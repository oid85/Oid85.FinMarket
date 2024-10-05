using Microsoft.Extensions.Configuration;
using Npgsql;
using Oid85.FinMarket.Application.Constants;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.AnalyseResults;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Helpers;
using Oid85.FinMarket.External.Settings;
using Oid85.FinMarket.External.Storage;
using Skender.Stock.Indicators;
using System.Text.Json;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.Application.Services
{
    /// <inheritdoc />
    public class AnalyseService : IAnalyseService
    {
        private readonly PostgresSqlHelper _sqlHelper;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISettingsService _settingsService;
        private readonly IStorageService _storageService;

        public AnalyseService(
            PostgresSqlHelper sqlHelper,
            ILogger logger,
            IConfiguration configuration,
            ISettingsService settingsService,
            IStorageService storageService)
        {
            _sqlHelper = sqlHelper ?? throw new ArgumentNullException(nameof(sqlHelper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        /// <inheritdoc />
        public async Task<List<AnalyseResult>> SupertrendAnalyseAsync(
            FinancicalInstrument stock, string timeframe)
        {
            try
            {
                await using var connection = await GetPostgresConnectionAsync();

                await connection.OpenAsync();                

                string tableName = $"{stock.Ticker}_{timeframe}".ToLower();

                var candles = await _storageService.GetCandlesAsync(tableName);

                await connection.CloseAsync();

                int lookbackPeriods = 50;
                double multiplier = 3.0;

                var quotes = candles
                    .Select(x => new Quote()
                    {
                        Open = Convert.ToDecimal(x.Open),
                        Close = Convert.ToDecimal(x.Close),
                        High = Convert.ToDecimal(x.High),
                        Low = Convert.ToDecimal(x.Low),
                        Date = x.Date
                    })
                    .ToList();

                var superTrendResults = quotes.GetSuperTrend(lookbackPeriods, multiplier);

                var results = superTrendResults
                    .Select(x => new AnalyseResult()
                    {
                        Date = x.Date,
                        Ticker = stock.Ticker,
                        Timeframe = timeframe,
                        TrendDirection = GetTrendDirection(x.Date, x.SuperTrend, candles),
                        Data = JsonSerializer.Serialize(x)
                    })
                    .ToList();

                await _storageService.SaveAnalyseResultsAsync(
                    $"{stock.Ticker}_supertrend_{KnownTimeframes.Daily}".ToLower(), 
                    results);

                return results;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
            }
        }

        private string GetTrendDirection(DateTime date, decimal? superTrend, List<Candle> candles)
        {
            if (superTrend == null)
                return string.Empty;

            var candle = candles.FirstOrDefault(x => x.Date == date);

            if (candle == null)
                return string.Empty;

            var minPrice = Math.Min(candle.Open, candle.Close);

            if (Convert.ToDouble((decimal) superTrend) < minPrice)
                return TrendDirectionConstants.Down;

            var maxPrice = Math.Max(candle.Open, candle.Close);

            if (Convert.ToDouble((decimal) superTrend) < maxPrice)
                return TrendDirectionConstants.Up;

            return string.Empty;
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
