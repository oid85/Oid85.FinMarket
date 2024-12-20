using Oid85.FinMarket.Application.Interfaces.Services;

namespace Oid85.FinMarket.Application.Services;

public class JobService(
    ILoadService loadService) 
    : IJobService
{
    public async Task LoadInstrumentsAsync()
    {
        await loadService.LoadStocksAsync();
        await loadService.LoadBondsAsync();
        await loadService.LoadFuturesAsync();
        await loadService.LoadCurrenciesAsync();
        await loadService.LoadIndicativesAsync();
    }

    public async Task LoadPricesAsync()
    {
        await loadService.LoadStockPricesAsync();
        await loadService.LoadBondPricesAsync();
        await loadService.LoadFuturePricesAsync();
        await loadService.LoadCurrencyPricesAsync();
    }
}