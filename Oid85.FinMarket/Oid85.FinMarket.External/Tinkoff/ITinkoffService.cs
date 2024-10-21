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
            FinInstrument instrument, string timeframe);

        /// <summary>
        /// Получить свечи за конкретный год
        /// </summary>
        /// <param name="instrument"> Финансовый инструмент</param>
        /// <param name="timeframe">Таймфрейм</param>
        /// <param name="year">Год</param>
        public Task<List<Candle>> GetCandlesAsync(
            FinInstrument instrument, string timeframe, int year);

        /// <summary>
        /// Получить список акций
        /// </summary>
        public Task<List<FinInstrument>> GetStocksAsync();

        /// <summary>
        /// Получить список облигаций
        /// </summary>
        public Task<List<FinInstrument>> GetBondsAsync();

        /// <summary>
        /// Получить список фьючерсов
        /// </summary>
        public Task<List<FinInstrument>> GetFuturesAsync();

        /// <summary>
        /// Получить список валют
        /// </summary>
        public Task<List<FinInstrument>> GetCurrenciesAsync();

        /// <summary>
        /// Получить информацию по дивидендам
        /// </summary>
        public Task<List<DividendInfo>> GetDividendInfoAsync(
            List<FinInstrument> instruments);
    }
}
