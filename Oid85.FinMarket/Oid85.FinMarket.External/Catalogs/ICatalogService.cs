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
        public Task<FinInstrument?> GetFinInstrumentAsync(
            string tableName, string ticker);

        /// <summary>
        /// Получить все активные финансовые инструменты
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        public Task<List<FinInstrument>> GetActiveFinInstrumentsAsync(
            string tableName);

        /// <summary>
        /// Получить информацию по дивидендам
        /// </summary>
        public Task<List<DividendInfo>> GetDividendInfosAsync();

        /// <summary>
        /// Обновить финансовые инструменты
        /// </summary>
        public Task UpdateFinInstrumentsAsync(
            string tableName, List<FinInstrument> instruments);

        /// <summary>
        /// Обновить информацию по дивидендам
        /// </summary>
        public Task UpdateDividendInfosAsync(List<DividendInfo> dividendInfos);

        /// <summary>
        /// Получить тикеры из индекса Мосбиржи
        /// </summary>
        public Task<List<MoexIndexStock>> GetMoexIndexStocksAsync();

        /// <summary>
        /// Получить тикеры из портфеля
        /// </summary>
        public Task<List<PortfolioStock>> GetPortfolioStocksAsync();

        /// <summary>
        /// Получить тикеры из списка наблюдения
        /// </summary>
        public Task<List<WatchListStock>> GetWatchListStocksAsync();
    }
}
