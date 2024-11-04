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
        public Task<List<Candle>> GetCandlesAsync(Share share, string timeframe);

        /// <summary>
        /// Получить свечи за конкретный год
        /// </summary>
        public Task<List<Candle>> GetCandlesAsync(Share share, string timeframe, int year);
        
        /// <summary>
        /// Получить список акций
        /// </summary>
        public Task<List<Share>> GetSharesAsync();

        /// <summary>
        /// Получить список облигаций
        /// </summary>
        public Task<List<Bond>> GetBondsAsync();

        /// <summary>
        /// Получить информацию по дивидендам
        /// </summary>
        public Task<List<DividendInfo>> GetDividendInfoAsync(List<Share> shares);
        
        /// <summary>
        /// Запрос купонов по облигации
        /// </summary>
        public Task<List<BondCoupon>> GetBondCouponsAsync(List<Bond> bonds);
    }
}
