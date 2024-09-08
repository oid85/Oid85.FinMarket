using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using NLog;

namespace Oid85.FinMarket.External.Settings
{
    public class SettingsService : ISettingsService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;

        public SettingsService(
            ILogger logger,
            IConfiguration configuration,
            IMemoryCache cache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        /// <inheritdoc />
        public async Task<T?> GetValueAsync<T>(string key)
        {
            try
            {
                var cachedValue = _cache.Get(key);

                if (cachedValue != null)
                    return (T) cachedValue;

                await using var connection = GetSqliteConnection();
                
                if (connection == null)
                {
                    _logger.Error("Не удалось установить соединение с БД data.db");
                    return default;
                }

                await connection.OpenAsync();

                var selectCommand = new SqliteCommand(
                    $"select value " +
                    $"from settings " +
                    $"where key = '{key}'", connection);

                object value = new();

                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                    if (reader.HasRows)
                        while (reader.Read())
                            value = reader.GetValue("value");

                    else
                    {
                        _logger.Error($"В таблице settings нет параметра '{key}'");
                        return default;
                    }

                await connection.CloseAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromHours(3));

                _cache.Set(key, value, cacheEntryOptions);

                return (T) value;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД data.db. {exception}");
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
                _logger.Error($"Не удалось установить соединение с БД data.db. {exception}");
                return null;
            }
        }
    }
}
