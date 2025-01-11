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

    public async Task LoadLastPricesAsync()
    {
        await loadService.LoadShareLastPricesAsync();
        await loadService.LoadBondLastPricesAsync();
        await loadService.LoadFutureLastPricesAsync();
        await loadService.LoadCurrencyLastPricesAsync();
        await loadService.LoadIndexLastPricesAsync();
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

    public async Task LoadCandlesAsync()
    {
        await loadService.LoadShareDailyCandlesAsync();
        await loadService.LoadBondDailyCandlesAsync();
        await loadService.LoadFutureDailyCandlesAsync();
        await loadService.LoadCurrencyDailyCandlesAsync();
        await loadService.LoadIndexDailyCandlesAsync();
        
        await loadService.LoadShareFiveMinuteCandlesAsync();
    }

    public async Task LoadFiveMinuteCandlesAsync()
    {
        await loadService.LoadShareFiveMinuteCandlesAsync();
    }

    public async Task AnalyseAsync()
    {
        await analyseService.AnalyseSharesAsync();
        await analyseService.AnalyseBondsAsync();
        await analyseService.AnalyseFuturesAsync();
        await analyseService.AnalyseCurrenciesAsync();
        await analyseService.AnalyseIndexesAsync();
    }

    public async Task FillingSpreadPairsAsync()
    {
        await spreadService.FillingSpreadPairsAsync();
    }

    public async Task CalculateSpreadsAsync()
    {
        await spreadService.CalculateSpreadsAsync();
    }
}