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

    public async Task LoadForecastsAsync()
    {
        await loadService.LoadForecastsAsync();
    }

    public async Task AnalyseAsync()
    {
        await analyseService.AnalyseSharesAsync();
        await analyseService.AnalyseBondsAsync();
        await analyseService.AnalyseFuturesAsync();
        await analyseService.AnalyseCurrenciesAsync();
        await analyseService.AnalyseIndexesAsync();
    }

    public async Task CalculateSpreadsAsync()
    {
        await spreadService.FillingSpreadPairsAsync();
        await spreadService.CalculateSpreadsAsync();
    }

    public async Task CalculateMultiplicatorsAsync()
    {
        await multiplicatorService.FillingMultiplicatorInstrumentsAsync();
        await multiplicatorService.CalculateMultiplicatorsAsync();
    }

    public async Task CheckMarketEventsAsync()
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
        await marketEventService.CheckCrossUpTargetPriceMarketEventAsync();
        await marketEventService.CheckCrossDownTargetPriceMarketEventAsync();
        await marketEventService.CheckSpreadGreaterPercent1MarketEventAsync();
        await marketEventService.CheckSpreadGreaterPercent2MarketEventAsync();
        await marketEventService.CheckSpreadGreaterPercent3MarketEventAsync();
    }

    public async Task SendNotificationsAsync()
    {
        await sendService.SendNotificationsAsync();
    }
}