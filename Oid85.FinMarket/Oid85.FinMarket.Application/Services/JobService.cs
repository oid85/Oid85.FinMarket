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
    public async Task<bool> LoadInstrumentsAsync()
    {
        await loadService.LoadSharesAsync();
        await loadService.LoadBondsAsync();
        await loadService.LoadFuturesAsync();
        await loadService.LoadCurrenciesAsync();
        await loadService.LoadIndexesAsync();

        return true;
    }

    public async Task<bool> LoadLastPricesAsync()
    {
        await loadService.LoadShareLastPricesAsync();
        await loadService.LoadBondLastPricesAsync();
        await loadService.LoadFutureLastPricesAsync();
        await loadService.LoadCurrencyLastPricesAsync();
        await loadService.LoadIndexLastPricesAsync();
        
        return true;
    }

    public async Task<bool> LoadBondCouponsAsync()
    {
        await loadService.LoadBondCouponsAsync();
        
        return true;
    }

    public async Task<bool> LoadDividendInfosAsync()
    {
        await loadService.LoadDividendInfosAsync();
        
        return true;
    }

    public async Task<bool> LoadAssetFundamentalsAsync()
    {
        await loadService.LoadAssetFundamentalsAsync();
        
        return true;
    }

    public async Task<bool> LoadCandlesAsync()
    {
        await loadService.LoadShareDailyCandlesAsync();
        await loadService.LoadBondDailyCandlesAsync();
        await loadService.LoadFutureDailyCandlesAsync();
        await loadService.LoadCurrencyDailyCandlesAsync();
        await loadService.LoadIndexDailyCandlesAsync();
        
        await loadService.LoadShareFiveMinuteCandlesAsync();
        
        return true;
    }

    public async Task<bool> LoadForecastsAsync()
    {
        await loadService.LoadForecastsAsync();
        
        return true;
    }

    public async Task<bool> AnalyseAsync()
    {
        await analyseService.AnalyseSharesAsync();
        await analyseService.AnalyseBondsAsync();
        await analyseService.AnalyseFuturesAsync();
        await analyseService.AnalyseCurrenciesAsync();
        await analyseService.AnalyseIndexesAsync();
        
        return true;
    }

    public async Task<bool> CalculateSpreadsAsync()
    {
        await spreadService.FillingSpreadPairsAsync();
        await spreadService.CalculateSpreadsAsync();
        
        return true;
    }

    public async Task<bool> CalculateMultiplicatorsAsync()
    {
        await multiplicatorService.FillingMultiplicatorInstrumentsAsync();
        await multiplicatorService.CalculateMultiplicatorsAsync();
        
        return true;
    }

    public async Task<bool> CheckMarketEventsAsync()
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
        
        return true;
    }

    public async Task<bool> SendNotificationsAsync()
    {
        return true;
    }
}