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
            _logger = logger;
            _settingsService = settingsService;
        }

        /// <inheritdoc />
        public async Task<FinancicalInstrument> GetFinancicalInstrumentAsync(string tableName, string ticker)
        {
            try
            {
                await using (var connection = await GetSqliteConnectionAsync())
                {
                    await connection!.OpenAsync();

                    var selectCommand = new SqliteCommand($"select id, ticker, figi, description, is_active from {tableName.ToLower()} where ticker = '{ticker}'", connection);

                    var financicalInstrument = new FinancicalInstrument();

                    using (SqliteDataReader reader = selectCommand.ExecuteReader())
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                financicalInstrument.Id = reader.GetInt32(0);
                                financicalInstrument.Ticker = reader.GetString(1);
                                financicalInstrument.Figi = reader.GetString(2);
                                financicalInstrument.Description = reader.GetString(3);
                                financicalInstrument.IsActive = reader.GetInt32(4);
                            }

                        else
                        {
                            string message = $"В таблице {tableName.ToLower()} нет инструмента '{ticker}'";
                            _logger.Error(message);
                            throw new Exception(message);
                        }

                    await connection.CloseAsync();

                    return financicalInstrument;
                }
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                throw new Exception(exception.Message);
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
                _logger.Error(exception);
                return null;
            }
        }
    }
}
