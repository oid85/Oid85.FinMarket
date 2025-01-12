namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис загрузки данных
/// </summary>
public interface ILoadService
{
    Task LoadSharesAsync();
    Task LoadShareLastPricesAsync();
    Task LoadShareDailyCandlesAsync();
    Task LoadShareFiveMinuteCandlesAsync();
    Task LoadForecastsAsync();
    
    Task LoadFuturesAsync();
    Task LoadFutureLastPricesAsync();
    Task LoadFutureDailyCandlesAsync();
    
    Task LoadBondsAsync();
    Task LoadBondCouponsAsync();
    Task LoadBondLastPricesAsync();
    Task LoadBondDailyCandlesAsync();
    
    Task LoadDividendInfosAsync();
    
    Task LoadIndexesAsync();
    Task LoadIndexLastPricesAsync();
    Task LoadIndexDailyCandlesAsync();
    
    Task LoadCurrenciesAsync();
    Task LoadCurrencyLastPricesAsync();
    Task LoadCurrencyDailyCandlesAsync();
    
    Task LoadAssetFundamentalsAsync();
}