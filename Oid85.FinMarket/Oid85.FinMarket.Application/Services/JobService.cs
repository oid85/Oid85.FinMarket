using Oid85.FinMarket.Application.Interfaces.Services;

namespace Oid85.FinMarket.Application.Services;

public class JobService(
    ILoadService loadService,
    IAnalyseService analyseService) 
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

    public async Task LoadBondCouponsAsync()
    {
        await loadService.LoadBondCouponsAsync();
    }

    public async Task LoadDividendInfosAsync()
    {
        await loadService.LoadDividendInfosAsync();
    }

    public async Task LoadAssetFundamentalsAsync()
    {
        await loadService.LoadAssetFundamentalsAsync();
    }

    public async Task LoadDailyCandlesAsync()
    {
        await loadService.LoadStockDailyCandlesAsync();
        await loadService.LoadFutureDailyCandlesAsync();
        await loadService.LoadIndicativeDailyCandlesAsync();
    }

    public async Task AnalyseAsync()
    {
        await analyseService.AnalyseStocksAsync();
        await analyseService.AnalyseIndexesAsync();
    }
}