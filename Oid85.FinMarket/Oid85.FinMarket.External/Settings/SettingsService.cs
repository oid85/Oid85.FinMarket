using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using NLog;
using Oid85.FinMarket.Domain.Models;

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
        public async Task<string> GetStringValueAsync(string key)
        {
            var value = (await GetValueAsync(key)).Value;

            return value.ToString()!;
        }

        /// <inheritdoc />
        public async Task<int> GetIntValueAsync(string key)
        {
            var value = (await GetValueAsync(key)).Value;

            return Convert.ToInt32(value);
        }

        /// <inheritdoc />
        public async Task<double> GetDoubleValueAsync(string key)
        {
            var value = (await GetValueAsync(key)).Value;

            return Convert.ToDouble(value);
        }

        /// <inheritdoc />
        public async Task<bool> GetBoolValueAsync(string key)
        {
            var value = (await GetValueAsync(key)).Value;

            return Convert.ToBoolean(value);
        }

        private async Task<SettingItem> GetValueAsync(string key)
        {
            try
            {
                var cachedValue = (SettingItem?) _cache.Get(key);

                if (cachedValue != null)
                    return cachedValue;

                await using var connection = GetSqliteConnection();

                await connection.OpenAsync();

                var item = (await connection
                    .QueryAsync<SettingItem>(
                        $"select id, key, value, description " +
                        $"from settings " +
                        $"where key = '{key}'"))
                    .FirstOrDefault();

                await connection.CloseAsync();

                if (item == null)
                    throw new InvalidOperationException($"{nameof(key)} - В таблице settings нет параметра '{key}'");

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromHours(3));

                _cache.Set(key, item, cacheEntryOptions);

                return item;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД data.db. {exception}");
                throw new InvalidOperationException($"Не удалось прочитать данные из БД data.db. {exception}");
            }
        }

        /// <summary>
        /// Получить соединение с БД
        /// </summary>
        private SqliteConnection GetSqliteConnection()
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
                throw new Exception($"Не удалось установить соединение с БД data.db. {exception}");
            }
        }
    }
}
