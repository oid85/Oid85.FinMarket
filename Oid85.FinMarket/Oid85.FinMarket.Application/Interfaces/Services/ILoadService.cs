namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис загрузки данных
/// </summary>
public interface ILoadService
{
    Task<bool> LoadSharesAsync();
    Task<bool> LoadShareLastPricesAsync();
    Task<bool> LoadShareDailyCandlesAsync();
    Task<bool> LoadShareFiveMinuteCandlesAsync();
    Task<bool> LoadForecastsAsync();
    Task<bool> LoadAssetReportEventsAsync();
    
    Task<bool> LoadFuturesAsync();
    Task<bool> LoadFutureLastPricesAsync();
    Task<bool> LoadFutureDailyCandlesAsync();
    
    Task<bool> LoadSpreadLastPricesAsync();
    
    Task<bool> LoadBondsAsync();
    Task<bool> LoadBondCouponsAsync();
    Task<bool> LoadBondLastPricesAsync();
    Task<bool> LoadBondDailyCandlesAsync();
    
    Task<bool> LoadDividendInfosAsync();
    
    Task<bool> LoadIndexesAsync();
    Task<bool> LoadIndexLastPricesAsync();
    Task<bool> LoadIndexDailyCandlesAsync();
    
    Task<bool> LoadCurrenciesAsync();
    Task<bool> LoadCurrencyLastPricesAsync();
    Task<bool> LoadCurrencyDailyCandlesAsync();
}