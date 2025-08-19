using NLog;
using Oid85.FinMarket.Application.Interfaces.Services;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class JobService(
    ILogger logger,
    ILoadService loadService,
    IImportService importService,
    IAnalyseService analyseService,
    IFeerGreedIndexService feerGreedIndexService,
    ISectorIndexService sectorIndexService,
    IMarketEventService marketEventService,
    ISendService sendService,
    IAlgoService algoService,
    IStatisticalArbitrageService statisticalArbitrageService) 
    : IJobService
{
    /// <inheritdoc />
    public async Task EarlyInTheDay()
    {
        await LoadInstrumentsAsync();
        await LoadLastPricesAsync();
        await LoadAssetReportEventsAsync();
        await LoadBondCouponsAsync();
        await LoadDividendInfosAsync();
        await LoadDailyCandlesAsync();
        await ImportAsync();        
        await LoadForecastsAsync();
        await AnalyseAsync();
        await CalculateSectorIndexDailyCandlesAsync();
        await AnalyseSectorsAsync();
        await ProcessFeerGreedAsync();
        await StatisticalArbitrationAsync();
        await CheckDailyMarketEventsAsync();
        await SendNotificationsAsync();
    }

    /// <inheritdoc />
    public async Task Every15Minutes()
    {

    }

    /// <inheritdoc />
    public async Task Every10Minutes()
    {

    }

    /// <inheritdoc />
    public async Task Every5Minutes()
    {

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
    
    private async Task LoadAssetReportEventsAsync()
        {
            try
            {
                await loadService.LoadAssetReportEventsAsync();
                
                logger.Info("Метод 'LoadAssetReportEventsAsync' выполнен успешно");
            }
            
            catch (Exception exception)
            {
                logger.Info(exception, "Ошибка при выполнении метода 'LoadAssetReportEventsAsync'");
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

    private async Task LoadHourlyCandlesAsync()
    {
        try
        {
            await loadService.LoadShareHourlyCandlesAsync();
            
            logger.Info("Метод 'LoadHourlyCandlesAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'LoadHourlyCandlesAsync'");
        }
    }    
    
    private async Task ImportAsync()
    {
        try
        {
            await importService.ImportMultiplicatorsAsync();
            
            logger.Info("Метод 'ImportAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'ImportAsync'");
        }
    }    
    
    private async Task OptimizeAsync()
    {
        try
        {
            await algoService.OptimizeAsync();
            
            logger.Info("Метод 'OptimizeAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'OptimizeAsync'");
        }
    }  
    
    private async Task BacktestAsync()
    {
        try
        {
            await algoService.BacktestAsync();
            
            logger.Info("Метод 'BacktestAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'BacktestAsync'");
        }
    }     
    
    private async Task StatisticalArbitrationAsync()
    {
        try
        {
            await statisticalArbitrageService.CalculateCorrelationAsync();
            await statisticalArbitrageService.CalculateRegressionTailsAsync();
            
            logger.Info("Метод 'StatisticalArbitrationAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'StatisticalArbitrationAsync'");
        }
    }       
    
    private async Task CalculateSectorIndexDailyCandlesAsync()
    {
        try
        {
            await sectorIndexService.CalculateOilAndGasSectorIndexDailyCandlesAsync();
            await sectorIndexService.CalculateBanksSectorIndexDailyCandlesAsync();
            await sectorIndexService.CalculateEnergSectorIndexDailyCandlesAsync();
            await sectorIndexService.CalculateFinanceSectorIndexDailyCandlesAsync();
            await sectorIndexService.CalculateHousingAndUtilitiesSectorIndexDailyCandlesAsync();
            await sectorIndexService.CalculateIronAndSteelIndustrySectorIndexDailyCandlesAsync();
            await sectorIndexService.CalculateItSectorIndexDailyCandlesAsync();
            await sectorIndexService.CalculateMiningSectorIndexDailyCandlesAsync();
            await sectorIndexService.CalculateNonFerrousMetallurgySectorIndexDailyCandlesAsync();
            await sectorIndexService.CalculateRetailSectorIndexDailyCandlesAsync();
            await sectorIndexService.CalculateTelecomSectorIndexDailyCandlesAsync();
            await sectorIndexService.CalculateTransportSectorIndexDailyCandlesAsync();
            
            logger.Info("Метод 'CalculateSectorIndexDailyCandlesAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'CalculateSectorIndexDailyCandlesAsync'");
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
            await analyseService.DailyAnalyseSharesAsync();
            await analyseService.DailyAnalyseBondsAsync();
            await analyseService.DailyAnalyseFuturesAsync();
            await analyseService.DailyAnalyseCurrenciesAsync();
            await analyseService.DailyAnalyseIndexesAsync();
            
            logger.Info("Метод 'AnalyseAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'AnalyseAsync'");
        }
    }

    private async Task AnalyseSectorsAsync()
    {
        try
        {
            await analyseService.DailyAnalyseOilAndGasSectorIndexAsync();
            await analyseService.DailyAnalyseBanksSectorIndexAsync();
            await analyseService.DailyAnalyseEnergSectorIndexAsync();
            await analyseService.DailyAnalyseFinanceSectorIndexAsync();
            await analyseService.DailyAnalyseHousingAndUtilitiesSectorIndexAsync();
            await analyseService.DailyAnalyseIronAndSteelIndustrySectorIndexAsync();
            await analyseService.DailyAnalyseItSectorIndexAsync();
            await analyseService.DailyAnalyseMiningSectorIndexAsync();
            await analyseService.DailyAnalyseNonFerrousMetallurgySectorIndexAsync();
            await analyseService.DailyAnalyseRetailSectorIndexAsync();
            await analyseService.DailyAnalyseTelecomSectorIndexAsync();
            await analyseService.DailyAnalyseTransportSectorIndexAsync();
            
            logger.Info("Метод 'AnalyseSectorsAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'AnalyseSectorsAsync'");
        }
    }    
    
    private async Task ProcessFeerGreedAsync()
    {
        try
        {
            await feerGreedIndexService.ProcessFeerGreedAsync();
            
            logger.Info("Метод 'ProcessFeerGreedAsync' выполнен успешно");
        }
        
        catch (Exception exception)
        {
            logger.Info(exception, "Ошибка при выполнении метода 'ProcessFeerGreedAsync'");
        }
    }
    
    private async Task CheckDailyMarketEventsAsync()
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
            await marketEventService.CheckForecastReleasedMarketEventAsync();
            
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