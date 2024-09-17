using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.External.Storage
{
    /// <summary>
    /// Сервис работы с хранилищем свечей
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Добавить свечи в хранилище
        /// </summary>
        /// <param name="data">Таблциы, свечи</param>        
        public Task SaveCandlesAsync(List<Tuple<string, List<Candle>>> data);

        /// <summary>
        /// Получить свечи из хранилища
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="count">Кол-во последних свечей</param>       
        public Task<List<Candle>> GetCandlesAsync(string tableName, int count);
    }
}
