namespace Oid85.FinMarket.External.Settings
{
    /// <summary>
    /// Сервис работы с настройками приложения
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Получить значение параметра настройки (string)
        /// </summary>
        public Task<string> GetStringValueAsync(string key);

        /// <summary>
        /// Получить значение параметра настройки (int)
        /// </summary>
        public Task<int> GetIntValueAsync(string key);

        /// <summary>
        /// Получить значение параметра настройки (double)
        /// </summary>
        public Task<double> GetDoubleValueAsync(string key);

        /// <summary>
        /// Получить значение параметра настройки (bool)
        /// </summary>
        public Task<bool> GetBoolValueAsync(string key);
    }
}
