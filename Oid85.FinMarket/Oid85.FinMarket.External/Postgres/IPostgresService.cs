using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.External.Postgres
{
    /// <summary>
    /// Сервис работы с хранилищем свечей
    /// </summary>
    public interface IPostgresService
    {
        /// <summary>
        /// Добавить свечи в хранилище
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="candles">Свечи фин. инструмента</param>
        /// <returns></returns>
        public int InsertCandles(string tableName, IEnumerable<Candle> candles);

        /// <summary>
        /// Получить свечи из хранилища
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="count">Кол-во последних свечей</param>
        /// <returns></returns>
        public IEnumerable<Candle> GetCandles(string tableName, int count);
    }
}
