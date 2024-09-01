using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.External.Tinkoff
{
    /// <summary>
    /// Сервис работы с источником данных от брокера Тинькофф Инвестиции
    /// </summary>
    public interface ITinkoffService
    {
        /// <summary>
        /// Получить свечи
        /// </summary>
        /// <param name="instrument"> Финансовый инструмент</param>
        /// <param name="timeframe">Таймфрейм</param>
        public Task<IList<Candle>> GetCandlesAsync(FinancicalInstrument instrument, string timeframe);
    }
}
