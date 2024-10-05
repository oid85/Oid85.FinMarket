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
        public Task<List<Candle>> GetCandlesAsync(
            FinancicalInstrument instrument, string timeframe);

        /// <summary>
        /// Получить свечи за конкретный год
        /// </summary>
        /// <param name="instrument"> Финансовый инструмент</param>
        /// <param name="timeframe">Таймфрейм</param>
        /// <param name="year">Год</param>
        public Task<List<Candle>> GetCandlesAsync(
            FinancicalInstrument instrument, string timeframe, int year);

        /// <summary>
        /// Получить список акций
        /// </summary>
        public List<FinancicalInstrument> GetStocks();

        /// <summary>
        /// Получить список облигаций
        /// </summary>
        public List<FinancicalInstrument> GetBonds();

        /// <summary>
        /// Получить список фьючерсов
        /// </summary>
        public List<FinancicalInstrument> GetFutures();

        /// <summary>
        /// Получить список валют
        /// </summary>
        public List<FinancicalInstrument> GetCurrencies();
    }
}
