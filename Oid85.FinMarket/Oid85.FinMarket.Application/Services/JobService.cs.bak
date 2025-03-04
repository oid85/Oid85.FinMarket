using NLog;
using Oid85.FinMarket.Application.Interfaces.Services;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class JobService(
    ILogger logger,
    ILoadService loadService,
    IAnalyseService analyseService,
    ISpreadService spreadService,
    IMultiplicatorService multiplicatorService,
    IMarketEventService marketEventService,
    ISendService sendService) 
    : IJobService
{
    /// <inheritdoc />
    public async Task EarlyInTheDay()
    {
        await LoadInstrumentsAsync();
        await LoadLastPricesAsync();
        await LoadBondCouponsAsync();
        await LoadDividendInfosAsync();
        await LoadDailyCandlesAsync();
        await LoadFiveMinuteCandlesAsync();
        await LoadForecastsAsync();
        await AnalyseAsync();
        await ProcessSpreadPairsAsync();
        await ProcessMultiplicatorsAsync();
        await CheckMarketEventsAsync();
        await SendNotificationsAsync();
    }

    /// <inheritdoc />
    public async Task Every15Minutes()
    {
        await LoadLastPricesAsync();
        await LoadFiveMinuteCandlesAsync();
        await ProcessSpreadPairsAsync();
        await CheckMarketEventsAsync();
        await SendNotificationsAsync();       
    }
    
    private async Task LoadInstrumentsAsync()
    {
        try
        {
            await loadService.LoadSharesAsync();
            await loadService.LoadBondsAsync();
            await loadService.LoadFuturesAsync();
            await loadService.LoadCurrenciesAsync();
            await loadService.LoadIndexesAsync();
            
            logger.Info("Метод 'LoadInstrumentsAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'LoadInstrumentsAsync'");
        }
    }

    private async Task LoadLastPricesAsync()
    {
        try
        {
            await loadService.LoadShareLastPricesAsync();
            await loadService.LoadBondLastPricesAsync();
            await loadService.LoadFutureLastPricesAsync();
            await loadService.LoadCurrencyLastPricesAsync();
            await loadService.LoadIndexLastPricesAsync();
            await loadService.LoadSpreadLastPricesAsync();
            
            logger.Info("Метод 'LoadLastPricesAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'LoadLastPricesAsync'");
        }
    }
    
    private async Task LoadBondCouponsAsync()
    {
        try
        {
            await loadService.LoadBondCouponsAsync();
            
            logger.Info("Метод 'LoadBondCouponsAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'LoadBondCouponsAsync'");
        }
    }

    private async Task LoadDividendInfosAsync()
    {
        try
        {
            await loadService.LoadDividendInfosAsync();
            
            logger.Info("Метод 'LoadDividendInfosAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'LoadDividendInfosAsync'");
        }
    }
    
    private async Task LoadDailyCandlesAsync()
    {
        try
        {
            await loadService.LoadShareDailyCandlesAsync();
            await loadService.LoadBondDailyCandlesAsync();
            await loadService.LoadFutureDailyCandlesAsync();
            await loadService.LoadCurrencyDailyCandlesAsync();
            await loadService.LoadIndexDailyCandlesAsync();
            
            logger.Info("Метод 'LoadDailyCandlesAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'LoadDailyCandlesAsync'");
        }
    }

    private async Task LoadFiveMinuteCandlesAsync()
    {
        try
        {
            await loadService.LoadShareFiveMinuteCandlesAsync();
            
            logger.Info("Метод 'LoadFiveMinuteCandlesAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'LoadFiveMinuteCandlesAsync'");
        }
    }
    
    private async Task LoadForecastsAsync()
    {
        try
        {
            await loadService.LoadForecastsAsync();
            
            logger.Info("Метод 'LoadForecastsAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'LoadForecastsAsync'");
        }
    }

    private async Task AnalyseAsync()
    {
        try
        {
            await analyseService.AnalyseSharesAsync();
            await analyseService.AnalyseBondsAsync();
            await analyseService.AnalyseFuturesAsync();
            await analyseService.AnalyseCurrenciesAsync();
            await analyseService.AnalyseIndexesAsync();
            
            logger.Info("Метод 'AnalyseAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'AnalyseAsync'");
        }
    }

    private async Task ProcessSpreadPairsAsync()
    {
        try
        {
            await spreadService.ProcessSpreadPairsAsync();
            
            logger.Info("Метод 'ProcessSpreadPairsAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'ProcessSpreadPairsAsync'");
        }
    }

    private async Task ProcessMultiplicatorsAsync()
    {
        try
        {
            await multiplicatorService.ProcessMultiplicatorsAsync();
            
            logger.Info("Метод 'ProcessMultiplicatorsAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'ProcessMultiplicatorsAsync'");
        }
    }

    private async Task CheckMarketEventsAsync()
    {
        try
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
            
            logger.Info("Метод 'CheckMarketEventsAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'CheckMarketEventsAsync'");
        }
    }

    private async Task SendNotificationsAsync()
    {
        try
        {
            await sendService.SendNotificationsAsync();
            
            logger.Info("Метод 'SendNotificationsAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'SendNotificationsAsync'");
        }
    }
}