using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using NLog;

namespace Oid85.FinMarket.External.Settings
{
    public class SettingsService : ISettingsService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public SettingsService(
            ILogger logger,
            IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc />
        public async Task<T?> GetValueAsync<T>(string key)
        {
            try
            {
                await using var connection = GetSqliteConnection();

                if (connection == null)
                {
                    _logger.Error("Не удалось установить соединение с БД settings.db");
                    return default;
                }

                var selectCommand = new SqliteCommand(
                    $"select value " +
                    $"from items " +
                    $"where key = '{key}'", connection);

                object value = new();

                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                    if (reader.HasRows)
                        while (reader.Read())
                            value = reader.GetValue("value");

                    else
                    {
                        _logger.Error($"В таблице items нет параметра '{key}'");
                        return default;
                    }

                await connection.CloseAsync();

                return (T)value;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД settings.db. {exception}");
                return default;
            }
        }

        /// <summary>
        /// Получить соединение с БД
        /// </summary>
        private SqliteConnection? GetSqliteConnection()
        {
            try
            {
                var connectionString = _configuration.GetValue<string>("SQLite:Settings:ConnectionString");
                var connection = new SqliteConnection(connectionString);

                return connection;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось установить соединение с БД settings.db. {exception}");
                return null;
            }
        }
    }
}
