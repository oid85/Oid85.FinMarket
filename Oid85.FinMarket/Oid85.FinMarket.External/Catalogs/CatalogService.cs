using System.Data;
using System.Net;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using NLog;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Settings;

namespace Oid85.FinMarket.External.Catalogs
{
    public class CatalogService : ICatalogService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISettingsService _settingsService;

        public CatalogService(
            ILogger logger,
            IConfiguration configuration,
            ISettingsService settingsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        /// <inheritdoc />
        public async Task<FinancicalInstrument?> GetFinancicalInstrumentAsync(string tableName, string ticker)
        {
            try
            {
                await using var connection = await GetSqliteConnectionAsync();

                if (connection == null)
                {
                    _logger.Error("Не удалось установить соединение с БД data.db");
                    return null;
                }

                await connection.OpenAsync();

                var selectCommand = new SqliteCommand(
                    $"select id, ticker, figi, description, sector, is_active " +
                    $"from {tableName.ToLower()} " +
                    $"where ticker = '{ticker}'", connection);

                var financicalInstrument = new FinancicalInstrument();

                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            financicalInstrument.Id = reader.GetInt32("id");
                            financicalInstrument.Ticker = reader.GetString("ticker");
                            financicalInstrument.Figi = reader.GetString("figi");
                            financicalInstrument.Description = reader.GetString("description");
                            financicalInstrument.Sector = reader.GetString("sector");
                            financicalInstrument.IsActive = reader.GetInt32("is_active");
                        }

                    else
                    {                        
                        _logger.Error($"В таблице {tableName.ToLower()} нет инструмента '{ticker}'");
                        return null;
                    }

                await connection.CloseAsync();

                return financicalInstrument;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД data.db. {exception}");
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<List<FinancicalInstrument>> GetActiveFinancicalInstrumentsAsync(string tableName)
        {
            try
            {
                await using var connection = await GetSqliteConnectionAsync();

                if (connection == null)
                {
                    _logger.Error("Не удалось установить соединение с БД data.db");
                    return [];
                }

                await connection.OpenAsync();

                var selectCommand = new SqliteCommand(
                    $"select id, ticker, figi, description, is_active " +
                    $"from {tableName.ToLower()} " +
                    $"where is_active = 1", connection);

                var financicalInstruments = new List<FinancicalInstrument>();
                
                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            var financicalInstrument = new FinancicalInstrument
                            {
                                Id = reader.GetInt32("id"),
                                Ticker = reader.GetString("ticker"),
                                Figi = reader.GetString("figi"),
                                Description = reader.GetString("description"),
                                IsActive = reader.GetInt32("is_active")
                            };

                            financicalInstruments.Add(financicalInstrument);
                        }

                    else
                    {
                        _logger.Error($"В таблице {tableName.ToLower()} нет активных инструментов");
                        return [];
                    }

                await connection.CloseAsync();

                return financicalInstruments;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД data.db. {exception}");
                return [];
            }
        }

        /// <inheritdoc />
        public async Task LoadFinancicalInstrumentsAsync(string tableName, List<FinancicalInstrument> instruments)
        {
            try
            {
                await using var connection = await GetSqliteConnectionAsync();

                if (connection == null)
                {
                    _logger.Error("Не удалось установить соединение с БД data.db");
                    return;
                }

                await connection.OpenAsync();

                foreach (var instrument in instruments) 
                {
                    string description = instrument.Description.Replace("'", "");

                    var command = new SqliteCommand(
                        $"insert into {tableName.ToLower()} (ticker, figi, description, sector, is_active) " +
                        $"values ('{instrument.Ticker}', '{instrument.Figi}', '{description}', " +
                        $"'{instrument.Sector}', {instrument.IsActive})", connection);

                    await command.ExecuteNonQueryAsync();
                }

                await connection.CloseAsync();
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД data.db. {exception}");
                return;
            }
        }

        /// <summary>
        /// Получить соединение с БД
        /// </summary>
        private async Task<SqliteConnection?> GetSqliteConnectionAsync()
        {
            try
            {
                var connectionString = _configuration.GetValue<string>("SQLite:Settings:ConnectionString");
                var connection = new SqliteConnection(connectionString);

                return connection;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось установить соединение с БД data.db. {exception}");
                return null;
            }
        }
    }
}
