using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.External.LiteDb
{
    /// <summary>
    /// Сервис работы с хранилищем справочников
    /// </summary>
    public interface ILiteDbService
    {
        /// <summary>
        /// Получить все финансовые инструменты
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FinancicalInstrument> GetFinancicalInstruments();
    }
}
