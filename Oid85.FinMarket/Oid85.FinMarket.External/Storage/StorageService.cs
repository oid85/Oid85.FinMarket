using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Helpers;
using NLog;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Oid85.FinMarket.External.Settings;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.External.Storage
{
    /// <inheritdoc />
    public class StorageService : IStorageService
    {
        private readonly PostgresSqlHelper _sqlHelper;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISettingsService _settingsService;

        public StorageService(
            PostgresSqlHelper sqlHelper,
            ILogger logger,
            IConfiguration configuration,
            ISettingsService settingsService)
        {
            _sqlHelper = sqlHelper ?? throw new ArgumentNullException(nameof(sqlHelper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        /// <inheritdoc />
        public async Task<int> SaveCandlesAsync(string tableName, IList<Candle> candles)
        {
            try
            {
                tableName = tableName.ToLower();

                int inserted = 0;

                await using var connection = await GetPostgresConnectionAsync();

                await connection.OpenAsync();

                // Если нет таблицы - создаем ее
                await _sqlHelper.NonQueryCommandAsync($"CREATE TABLE IF NOT EXISTS {tableName} (" +
                    $"id bigserial NOT NULL, " +
                    $"\"open\" double precision NULL, " +
                    $"\"close\" double precision NULL, " +
                    $"high double precision NULL, " +
                    $"low double precision NULL, " +
                    $"volume bigint NULL, " +
                    $"\"date\" timestamp with time zone NULL, " +
                    $"is_complete int NULL, " +
                    $"CONSTRAINT {tableName}_pk PRIMARY KEY (id));",
                    connection);

                for (var i = 0; i < candles.Count; i++)
                {
                    var (open, close, high, low) = FormatCandle(candles[i]);

                    // Если свеча уже записана в хранилище
                    var alreadyExistCandleTable = _sqlHelper.Select(
                        $"select date, is_complete " +
                        $"from {tableName} " +
                        $"where date = '{candles[i].Date}'", connection);

                    if (alreadyExistCandleTable!.Rows.Count > 0)
                    {
                        bool isComplete = Convert.ToInt32(alreadyExistCandleTable.Rows[0]["is_complete"]) == 1;

                        // Ессли свеча не полная, то обновляем ее
                        if (!isComplete)
                            await _sqlHelper.NonQueryCommandAsync(
                                $"update finmarket " +
                                $"set open = {open}, close = {close}, high = {high}, low = {low}, " +
                                $"volume = {candles[i].Volume}, is_complete = {candles[i].IsComplete} " +
                                $"where date = '{candles[i].Date}')",
                                connection);
                    }

                    else
                    {
                        // Если свеча еще не записана в хранилище
                        await _sqlHelper.NonQueryCommandAsync(
                            $"insert into {tableName} (open, close, high, low, volume, date, is_complete) " +
                            $"values ({open}, {close}, {high}, {low}, {candles[i].Volume}, '{candles[i].Date}', {candles[i].IsComplete})",
                            connection);
                    }

                    inserted++;
                }

                await connection.CloseAsync();

                return inserted;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
            }
        }

        /// <inheritdoc />
        public async Task<List<Candle>> GetCandlesAsync(string tableName, int count)
        {
            try
            {
                await using var connection = await GetPostgresConnectionAsync();

                await connection.OpenAsync();

                var table = _sqlHelper.Select(
                    $"select open, close, high, low, volume, date " +
                    $"from {tableName} " +
                    $"order by date " +
                    $"limit {count}", connection);

                await connection.CloseAsync();

                var candles = new List<Candle>();

                for (int i = 0; i < table!.Rows.Count; i++)
                {
                    var open = Convert.ToDouble(table.Rows[i][0]);
                    var close = Convert.ToDouble(table.Rows[i][1]);
                    var high = Convert.ToDouble(table.Rows[i][2]);
                    var low = Convert.ToDouble(table.Rows[i][3]);
                    var volume = Convert.ToInt32(table.Rows[i][4]);
                    var date = Convert.ToDateTime(table.Rows[i][5]);

                    var candle = new Candle()
                    {
                        Open = open,
                        Close = close,
                        High = high,
                        Low = low,
                        Volume = volume,
                        Date = date
                    };

                    candles.Add(candle);
                }

                return candles;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
            }
        }

        private (string, string, string, string) FormatCandle(Candle candle)
        {
            string open = candle.Open.ToString().Replace(',', '.');
            string close = candle.Close.ToString().Replace(',', '.');
            string high = candle.High.ToString().Replace(',', '.');
            string low = candle.Low.ToString().Replace(',', '.');

            return (open, close, high, low);
        }

        /// <summary>
        /// Получить соединение с БД
        /// </summary>
        private async Task<NpgsqlConnection> GetPostgresConnectionAsync()
        {
            try
            {
                var connectionString = await _settingsService.GetStringValueAsync(KnownSettingsKeys.Postgres_ConnectionString);
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
