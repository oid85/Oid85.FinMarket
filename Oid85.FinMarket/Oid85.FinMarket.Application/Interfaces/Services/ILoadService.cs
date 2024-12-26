using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Services;

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
    Task LoadAssetFundamentalsAsync();
}