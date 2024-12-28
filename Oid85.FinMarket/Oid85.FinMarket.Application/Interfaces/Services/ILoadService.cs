namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис загрузки данных
/// </summary>
public interface ILoadService
{
    Task LoadStocksAsync();
    Task LoadStockPricesAsync();
    Task LoadStockDailyCandlesAsync();
    
    Task LoadFuturesAsync();
    Task LoadFuturePricesAsync();
    Task LoadFutureDailyCandlesAsync();
    
    Task LoadBondsAsync();
    Task LoadBondCouponsAsync();
    Task LoadBondPricesAsync();
    Task LoadBondDailyCandlesAsync();
    
    Task LoadDividendInfosAsync();
    
    Task LoadIndicativesAsync();
    Task LoadIndicativePricesAsync();
    Task LoadIndicativeDailyCandlesAsync();
    
    Task LoadCurrenciesAsync();
    Task LoadCurrencyPricesAsync();
    Task LoadCurrencyDailyCandlesAsync();
    
    Task LoadAssetFundamentalsAsync();
}