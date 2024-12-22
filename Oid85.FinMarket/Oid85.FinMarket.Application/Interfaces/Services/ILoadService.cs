using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Services;

public interface ILoadService
{
    Task LoadStocksAsync();
    Task LoadStockPricesAsync();
    Task LoadStockDailyCandlesAsync();
    Task LoadFuturesAsync();
    Task LoadFuturePricesAsync();
    Task LoadBondsAsync();
    Task LoadBondCouponsAsync();
    Task LoadBondPricesAsync();
    Task LoadDividendInfosAsync();
    Task LoadIndicativesAsync();
    Task LoadIndicativePricesAsync();
    Task LoadCurrenciesAsync();
    Task LoadCurrencyPricesAsync();
}