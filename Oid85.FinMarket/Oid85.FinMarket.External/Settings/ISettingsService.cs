namespace Oid85.FinMarket.External.Settings
{
    /// <summary>
    /// Сервис работы с настройками приложения
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Получить значение параметра настройки
        /// </summary>
        public Task<T?> GetValueAsync<T>(string key);
    }
}
