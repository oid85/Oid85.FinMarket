using Oid85.FinMarket.Domain.AnalyseResults;
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
        /// Получить все свечи из хранилища
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>    
        public Task<List<Candle>> GetCandlesAsync(string tableName);

        /// <summary>
        /// Получить последние N свечей из хранилища
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="count">Кол-во последних свечей</param>       
        public Task<List<Candle>> GetCandlesAsync(string tableName, int count);

        /// <summary>
        /// Получить последние N свечей до определенной даты из хранилища
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="count">Кол-во последних свечей</param>       
        /// <param name="dateTime">Дата последней запрашиваемой свечи</param>  
        public Task<List<Candle>> GetCandlesAsync(string tableName, int count, DateTime dateTime);

        /// <summary>
        /// Сохранить результаты анализа
        /// </summary>
        public Task SaveAnalyseResultsAsync(string tableName, List<AnalyseResult> results);
    }
}
