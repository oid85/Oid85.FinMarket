namespace Oid85.FinMarket.Application.Interfaces.Services
{
    public interface ILoadService
    {
        Task LoadStocksAsync();
        Task LoadBondsAsync();
        Task LoadCandlesAsync();
        Task LoadCandlesAsync(int year);
        Task LoadDividendInfosAsync();
        Task LoadBondCouponsAsync();
    }
}
