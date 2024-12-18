using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Services
{
    public interface ILoadService
    {
        Task LoadStocksAsync();
        Task LoadStockPricesAsync();
        Task LoadStocksDailyCandlesAsync();
        Task LoadStocksDailyCandlesAsync(Share share);
        Task LoadStocksDailyCandlesAsync(int year);
        Task LoadFuturesAsync();
        Task LoadFuturePricesAsync();
        Task LoadFuturesDailyCandlesAsync();
        Task LoadFuturesDailyCandlesAsync(Future future);
        Task LoadFuturesDailyCandlesAsync(int year);
        Task LoadBondsAsync();
        Task LoadBondCouponsAsync();
        Task LoadBondPricesAsync();
        Task LoadDividendInfosAsync();
        Task LoadIndicativesAsync();
        Task LoadIndicativePricesAsync();
    }
}
