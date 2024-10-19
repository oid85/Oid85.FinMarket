namespace Oid85.FinMarket.Application.Services
{
    public interface ILoadService
    {
        Task LoadStocksCatalogAsync();
        Task LoadBondsCatalogAsync();
        Task LoadFuturesCatalogAsync();
        Task LoadCurrenciesCatalogAsync();
        Task LoadStocksDailyCandlesAsync();
        Task LoadStocksDailyCandlesForYearAsync(int year);
    }
}
