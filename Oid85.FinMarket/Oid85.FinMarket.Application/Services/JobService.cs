using Oid85.FinMarket.Application.Interfaces.Services;

namespace Oid85.FinMarket.Application.Services;

public class JobService(
    ILoadService loadService,
    IAnalyseService analyseService,
    ISpreadService spreadService) 
    : IJobService
{
    public async Task LoadInstrumentsAsync()
    {
        await loadService.LoadSharesAsync();
        await loadService.LoadBondsAsync();
        await loadService.LoadFuturesAsync();
        await loadService.LoadCurrenciesAsync();
        await loadService.LoadIndexesAsync();
    }

    public async Task LoadPricesAsync()
    {
        await loadService.LoadShareLastPricesAsync();
        await loadService.LoadBondLastPricesAsync();
        await loadService.LoadFutureLastPricesAsync();
        await loadService.LoadCurrencyLastPricesAsync();
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
        await loadService.LoadShareDailyCandlesAsync();
        await loadService.LoadFutureDailyCandlesAsync();
        await loadService.LoadIndexDailyCandlesAsync();
    }

    public async Task AnalyseAsync()
    {
        await analyseService.AnalyseSharesAsync();
        await analyseService.AnalyseIndexesAsync();
    }

    public async Task CalculateSpreadsAsync()
    {
        await spreadService.CalculateSpreadsAsync();
    }
}