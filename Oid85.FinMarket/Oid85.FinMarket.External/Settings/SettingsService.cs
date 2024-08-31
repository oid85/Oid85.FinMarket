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
            _logger = logger;
            _configuration = configuration;
        }

        /// <inheritdoc />
        public async Task<T> GetValueAsync<T>(string key)
        {
            try
            {
                await using (var connection = GetSqliteConnection())
                {
                    await connection!.OpenAsync();

                    var selectCommand = new SqliteCommand($"select value from items where key = '{key}'", connection);

                    object value = new();

                    using (SqliteDataReader reader = selectCommand.ExecuteReader())
                        if (reader.HasRows)
                            while (reader.Read())
                                value = reader.GetValue(0);

                        else
                        {
                            string message = $"В таблице items нет параметра '{key}'";                            
                            _logger.Error(message);
                            throw new Exception(message);
                        }

                    await connection.CloseAsync();

                    return (T) value;
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
                _logger.Error(exception);
                return null;
            }
        }
    }
}
