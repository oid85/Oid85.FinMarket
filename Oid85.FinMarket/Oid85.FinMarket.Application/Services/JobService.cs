using Oid85.FinMarket.Application.Interfaces.Services;

namespace Oid85.FinMarket.Application.Services;

public class JobService(
    ILoadService loadService,
    IAnalyseService analyseService,
    ISpreadService spreadService,
    IMultiplicatorService multiplicatorService,
    IMarketEventService marketEventService,
    ISendService sendService) 
    : IJobService
{
    /// <inheritdoc />
    public async Task<bool> LoadAsync()
    {
        await LoadInstrumentsAsync();
        await LoadLastPricesAsync();
        await LoadBondCouponsAsync();
        await LoadDividendInfosAsync();
        await LoadAssetFundamentalsAsync();
        await LoadDailyCandlesAsync();
        await LoadFiveMinuteCandlesAsync();
        await LoadForecastsAsync();
        
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> CalculateAsync()
    {
        await AnalyseAsync();
        await CalculateSpreadsAsync();
        await CalculateMultiplicatorsAsync();
        await CheckMarketEventsAsync();
        await SendNotificationsAsync();
        
        return true;
    }
    
    private async Task LoadInstrumentsAsync()
    {
        await loadService.LoadSharesAsync();
        await loadService.LoadBondsAsync();
        await loadService.LoadFuturesAsync();
        await loadService.LoadCurrenciesAsync();
        await loadService.LoadIndexesAsync();
    }

    private async Task LoadLastPricesAsync()
    {
        await loadService.LoadShareLastPricesAsync();
        await loadService.LoadBondLastPricesAsync();
        await loadService.LoadFutureLastPricesAsync();
        await loadService.LoadCurrencyLastPricesAsync();
        await loadService.LoadIndexLastPricesAsync();
    }

    private async Task LoadBondCouponsAsync()
    {
        await loadService.LoadBondCouponsAsync();
    }

    private async Task LoadDividendInfosAsync()
    {
        await loadService.LoadDividendInfosAsync();
    }

    private async Task LoadAssetFundamentalsAsync()
    {
        await loadService.LoadAssetFundamentalsAsync();
    }

    private async Task LoadDailyCandlesAsync()
    {
        await loadService.LoadShareDailyCandlesAsync();
        await loadService.LoadBondDailyCandlesAsync();
        await loadService.LoadFutureDailyCandlesAsync();
        await loadService.LoadCurrencyDailyCandlesAsync();
        await loadService.LoadIndexDailyCandlesAsync();
    }

    private async Task LoadFiveMinuteCandlesAsync()
    {
        await loadService.LoadShareFiveMinuteCandlesAsync();
    }
    
    private async Task LoadForecastsAsync()
    {
        await loadService.LoadForecastsAsync();
    }

    private async Task AnalyseAsync()
    {
        await analyseService.AnalyseSharesAsync();
        await analyseService.AnalyseBondsAsync();
        await analyseService.AnalyseFuturesAsync();
        await analyseService.AnalyseCurrenciesAsync();
        await analyseService.AnalyseIndexesAsync();
    }

    private async Task CalculateSpreadsAsync()
    {
        await spreadService.FillingSpreadPairsAsync();
        await spreadService.CalculateSpreadsAsync();
    }

    private async Task CalculateMultiplicatorsAsync()
    {
        await multiplicatorService.FillingMultiplicatorInstrumentsAsync();
        await multiplicatorService.CalculateMultiplicatorsAsync();
    }

    private async Task CheckMarketEventsAsync()
    {
        await marketEventService.CheckSupertrendUpMarketEventAsync();
        await marketEventService.CheckSupertrendDownMarketEventAsync();
        await marketEventService.CheckCandleVolumeUpMarketEventAsync();
        await marketEventService.CheckCandleSequenceWhiteMarketEventAsync();
        await marketEventService.CheckCandleSequenceBlackMarketEventAsync();
        await marketEventService.CheckRsiOverBoughtInputMarketEventAsync();
        await marketEventService.CheckRsiOverBoughtOutputMarketEventAsync();
        await marketEventService.CheckRsiOverOverSoldInputMarketEventAsync();
        await marketEventService.CheckRsiOverOverSoldOutputMarketEventAsync();
        await marketEventService.CheckCrossPriceLevelMarketEventAsync();
        await marketEventService.CheckSpreadGreaterPercent1MarketEventAsync();
        await marketEventService.CheckSpreadGreaterPercent2MarketEventAsync();
        await marketEventService.CheckSpreadGreaterPercent3MarketEventAsync();
    }

    private async Task SendNotificationsAsync()
    {
        await sendService.SendNotificationsAsync();
    }
}