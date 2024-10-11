using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.External.Catalogs
{
    /// <summary>
    /// Сервис работы со справичниками
    /// </summary>
    public interface ICatalogService
    {
        /// <summary>
        /// Получить финансовый инструмент
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="ticker">Тикер</param>
        public Task<FinancicalInstrument?> GetFinancicalInstrumentAsync(
            string tableName, string ticker);

        /// <summary>
        /// Получить все активные финансовые инструменты
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        public Task<List<FinancicalInstrument>> GetActiveFinancicalInstrumentsAsync(
            string tableName);

        /// <summary>
        /// Обновить финансовые инструменты
        /// </summary>
        public Task UpdateFinancicalInstrumentsAsync(
            string tableName, List<FinancicalInstrument> instruments);

        /// <summary>
        /// Получить тикеры из индекса Мосбиржи
        /// </summary>
        public Task<List<MoexIndexItem>> GetMoexIndexItemsAsync();

        /// <summary>
        /// Получить тикеры из портфеля
        /// </summary>
        public Task<List<PortfolioItem>> GetPortfolioItemsAsync();

        /// <summary>
        /// Получить тикеры из списка наблюдения
        /// </summary>
        public Task<List<WatchListItem>> GetWatchListItemsAsync();
    }
}
