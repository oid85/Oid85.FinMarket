using System.Data;
using Microsoft.Data.Sqlite;
using NLog;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Settings;

namespace Oid85.FinMarket.External.Catalogs
{
    public class CatalogService : ICatalogService
    {
        private readonly ILogger _logger;
        private readonly ISettingsService _settingsService;

        public CatalogService(
            ILogger logger,
            ISettingsService settingsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                    _logger.Error("Не удалось установить соединение с БД catalogs.db");
                    return null;
                }

                await connection.OpenAsync();

                var selectCommand = new SqliteCommand(
                    $"select id, ticker, figi, description, is_active " +
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
                _logger.Error($"Не удалось прочитать данные из БД catalogs.db. {exception}");
                return null;
            }
        }

        /// <summary>
        /// Получить соединение с БД
        /// </summary>
        private async Task<SqliteConnection?> GetSqliteConnectionAsync()
        {
            try
            {
                var connectionString = await _settingsService.GetValueAsync<string>(KnownSettingsKeys.SQLite_Catalogs_ConnectionString);
                var connection = new SqliteConnection(connectionString);

                return connection;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось установить соединение с БД catalogs.db. {exception}");
                return null;
            }
        }
    }
}
