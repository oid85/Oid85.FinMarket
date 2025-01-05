using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.External.Tinkoff;

/// <summary>
/// Сервис работы с источником данных от брокера Тинькофф Инвестиции
/// </summary>
public interface ITinkoffService
{       
    /// <summary>
    /// Получить свечи
    /// </summary>
    public Task<List<Candle>> GetCandlesAsync(
        Guid instrumentId, DateOnly from, DateOnly to);
    
    /// <summary>
    /// Получить свечи за конкретный год
    /// </summary>
    public Task<List<Candle>> GetCandlesAsync(Guid instrumentId, int year);
        
    /// <summary>
    /// Получить последние цены
    /// </summary>
    public Task<List<double>> GetPricesAsync(List<Guid> instrumentIds);
        
    /// <summary>
    /// Получить список акций
    /// </summary>
    public Task<List<Share>> GetSharesAsync();

    /// <summary>
    /// Получить список фьючерсов
    /// </summary>
    public Task<List<Future>> GetFuturesAsync();        
        
    /// <summary>
    /// Получить список облигаций
    /// </summary>
    public Task<List<Bond>> GetBondsAsync();

    /// <summary>
    /// Получить список индексов
    /// </summary>
    public Task<List<FinIndex>> GetIndexesAsync();
        
    /// <summary>
    /// Получить список валют
    /// </summary>
    public Task<List<Currency>> GetCurrenciesAsync();
        
    /// <summary>
    /// Получить информацию по дивидендам
    /// </summary>
    public Task<List<DividendInfo>> GetDividendInfoAsync(List<Share> shares);
        
    /// <summary>
    /// Получить купоны по облигациям
    /// </summary>
    public Task<List<BondCoupon>> GetBondCouponsAsync(List<Bond> bonds);
    
    /// <summary>
    /// Получить фундаментальные данные
    /// </summary>
    public Task<List<AssetFundamental>> GetAssetFundamentalsAsync(List<Guid> instrumentIds);
}