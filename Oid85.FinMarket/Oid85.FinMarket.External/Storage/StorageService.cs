using Oid85.FinMarket.Domain.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Oid85.FinMarket.External.Settings;
using Oid85.FinMarket.Common.KnownConstants;
using Dapper;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.External.Storage
{
    /// <inheritdoc />
    public class StorageService : IStorageService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISettingsService _settingsService;

        public StorageService(
            ILogger logger,
            IConfiguration configuration,
            ISettingsService settingsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        /// <inheritdoc />
        public async Task SaveCandlesAsync(List<Tuple<string, List<Candle>>> data)
        {
            try
            {
                await using var connection = await GetPostgresConnectionAsync();

                await connection.OpenAsync();

                foreach (var item in data)
                    await SaveCandlesAsync(item.Item1, item.Item2, connection);

                await connection.CloseAsync();
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
            }
        }

        /// <inheritdoc />
        public async Task<List<Candle>> GetCandlesAsync(string tableName)
        {
            try
            {
                await using var connection = await GetPostgresConnectionAsync();

                await connection.OpenAsync();

                var candles = (await connection
                    .QueryAsync<Candle>(
                        $"select open, close, high, low, volume, date " +
                        $"from {tableName} " +
                        $"order by date"))
                        .OrderBy(x => x.Date)
                        .ToList();

                await connection.CloseAsync();

                return candles;
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

                var candles = (await connection
                    .QueryAsync<Candle>(
                        $"select open, close, high, low, volume, date " +
                        $"from {tableName} " +
                        $"order by date " + 
                        $"limit {count}"))
                        .OrderBy(x => x.Date)
                        .ToList();

                await connection.CloseAsync();

                return candles;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
            }
        }

        /// <inheritdoc />
        public async Task<List<Candle>> GetCandlesAsync(
            string tableName, int count, DateTime dateTime)
        {
            try
            {
                await using var connection = await GetPostgresConnectionAsync();

                await connection.OpenAsync();

                var candles = (await connection
                    .QueryAsync<Candle>(
                        $"select open, close, high, low, volume, date " +
                        $"from {tableName} " +
                        $"where date <= '{dateTime}' " +
                        $"order by date " +
                        $"limit {count}"))
                        .OrderBy(x => x.Date)
                        .ToList();

                await connection.CloseAsync();

                return candles;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
            }
        }

        /// <inheritdoc />
        public async Task<List<AnalyseResult>> GetAnalyseResultsAsync(
            string tableName, DateTime from, DateTime to)
        {
            try
            {
                await using var connection = await GetPostgresConnectionAsync();

                await connection.OpenAsync();

                var analyseResults = (await connection
                    .QueryAsync<AnalyseResult>(
                        $"select id, ticker, timeframe, trend_direction as trenddirection, data, date " +
                        $"from {tableName} " +
                        $"where date >= '{from}' " +
                        $"and date <= '{to}' " +
                        $"order by date"))
                        .OrderBy(x => x.Date)
                        .ToList();

                await connection.CloseAsync();

                return analyseResults;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                return [];
            }
        }

        /// <inheritdoc />
        public async Task SaveAnalyseResultsAsync(
            string tableName, List<AnalyseResult> results)
        {
            try
            {
                await using var connection = await GetPostgresConnectionAsync();

                await connection.OpenAsync();

                // Если нет таблицы - создаем ее
                await CreateAnalyseTableIfNotExistsAsync(tableName, connection);

                foreach (var result in results)
                    await SaveAnalyseResultAsync(tableName, result, connection);

                await connection.CloseAsync();
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
        /// Сохранить один аналитический результат
        /// </summary>
        private async Task<int> SaveAnalyseResultAsync(
            string tableName,
            AnalyseResult result, 
            NpgsqlConnection connection)
        {
            try
            {
                tableName = tableName.ToLower();                

                int inserted = 0;

                DateTime date = result.Date;

                // Если аналитический результат уже записан в хранилище
                var existedAnalyseResult = (await connection
                    .QueryAsync<AnalyseResult>(
                        $"select id, ticker, timeframe, trend_direction, data, date " +
                        $"from {tableName} " +
                        $"where date = '{date}' "))
                    .FirstOrDefault();

                if (existedAnalyseResult is null)
                {
                    // Если результат еще не записан в хранилище
                    await connection.ExecuteAsync(
                        $"insert into {tableName} " +
                        $"(ticker, timeframe, trend_direction, data, date) " +
                        $"values (" +
                        $"'{result.Ticker}', " +
                        $"'{result.Timeframe}', " +
                        $"'{result.TrendDirection}', " +
                        $"'{result.Data}', " +
                        $"'{result.Date}')");
                }

                else
                {
                    await connection.ExecuteAsync(
                        $"update {tableName} " +
                        $"set timeframe = '{result.Timeframe}', " +
                        $"trend_direction = '{result.TrendDirection}', " +
                        $"data = '{result.Data}', " +
                        $"date = '{result.Date}'" +
                        $"where ticker = '{result.Ticker}'");
                }

                inserted++;

                return inserted;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                return -1;
            }
        }

        /// <summary>
        /// Сохранить свечи по одному инструменту
        /// </summary>
        private async Task<int> SaveCandlesAsync(
            string tableName,
            List<Candle> candles,
            NpgsqlConnection connection)
        {
            try
            {
                // Если нет таблицы - создаем ее
                await CreateCandlesTableIfNotExistsAsync(tableName, connection);

                int inserted = 0;

                for (var i = 0; i < candles.Count; i++)
                {
                    var (open, close, high, low) = FormatCandle(candles[i]);

                    var date = candles[i].Date;
                    var volume = candles[i].Volume;
                    var isComplete = candles[i].IsComplete;

                    // Если свеча уже записана в хранилище
                    var existedCandle = (await connection
                        .QueryAsync<Candle>(
                            $"select id, open, close, high, low, volume, date, is_complete " +
                            $"from {tableName} " +
                            $"where date = '{date}'"))
                        .FirstOrDefault();

                    if (existedCandle is null)
                    {
                        // Если свеча еще не записана в хранилище
                        await connection.ExecuteAsync(
                            $"insert into {tableName} " +
                            $"(open, close, high, low, volume, date, is_complete) " +
                            $"values (" +
                            $"{open}, {close}, {high}, {low}, " +
                            $"{volume}, " +
                            $"'{date}', " +
                            $"{isComplete})");
                    }

                    else
                    {
                        long id = existedCandle.Id;

                        // Если свеча не полная, то обновляем ее
                        if (existedCandle.IsComplete == 0)
                            await connection.ExecuteAsync(
                                $"update {tableName} " +
                                $"set open = {open}, " +
                                $"close = {close}, " +
                                $"high = {high}, " +
                                $"low = {low}, " +
                                $"volume = {volume}, " +
                                $"is_complete = {isComplete} " +
                                $"where id = {id}");
                    }

                    inserted++;
                }

                return inserted;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                return -1;
            }
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

        private async Task CreateCandlesTableIfNotExistsAsync(
            string tableName, NpgsqlConnection connection)
        {
            tableName = tableName.ToLower();

            await connection.ExecuteAsync(
                $"CREATE TABLE IF NOT EXISTS {tableName} (" +
                $"id bigserial NOT NULL, " +
                $"open double precision NULL, " +
                $"close double precision NULL, " +
                $"high double precision NULL, " +
                $"low double precision NULL, " +
                $"volume bigint NULL, " +
                $"date timestamp with time zone NULL, " +
                $"is_complete int NULL, " +
                $"CONSTRAINT {tableName}_pk PRIMARY KEY (id))");
        }

        private async Task CreateAnalyseTableIfNotExistsAsync(
            string tableName, NpgsqlConnection connection)
        {
            tableName = tableName.ToLower();

            await connection.ExecuteAsync(
                $"CREATE TABLE IF NOT EXISTS {tableName} (" +
                $"id bigserial NOT NULL, " +
                $"ticker text NULL, " +
                $"timeframe text NULL, " +
                $"trend_direction text NULL, " +
                $"data text NULL, " +
                $"date timestamp with time zone NULL, " +
                $"CONSTRAINT {tableName}_pk PRIMARY KEY (id))");
        }
    }
}
