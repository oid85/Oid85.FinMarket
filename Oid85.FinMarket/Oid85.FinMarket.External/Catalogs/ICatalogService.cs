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
        public Task<FinancicalInstrument?> GetFinancicalInstrumentAsync(string tableName, string ticker);
    }
}
