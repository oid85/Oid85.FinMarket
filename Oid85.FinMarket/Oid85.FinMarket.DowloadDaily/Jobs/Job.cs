using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Settings;
using Oid85.FinMarket.External.Storage;
using Oid85.FinMarket.External.Tinkoff;
using Quartz;
using ILogger = NLog.ILogger;

namespace DaGroup.Mfsb.Computation.WebHost.Jobs
{
    [DisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public class Job : IJob
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISettingsService _settingsService;
        private readonly ITinkoffService _tinkoffService;
        private readonly IStorageService _storageService;
        private readonly ICatalogService _catalogService;

        public Job(
            ILogger logger, 
            IServiceProvider serviceProvider,
            ISettingsService settingsService,
            ITinkoffService tinkoffService,
            IStorageService storageService,
            ICatalogService catalogService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _tinkoffService = tinkoffService ?? throw new ArgumentNullException(nameof(tinkoffService));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var enabled = await _settingsService.GetBoolValueAsync(KnownSettingsKeys.Quartz_DowloadDaily_Enable);

            if (!enabled)
                return;

            var stocks = await _catalogService
                .GetActiveFinancicalInstrumentsAsync(KnownFinancicalInstrumentTypes.Stocks);

            foreach (var stock in stocks) 
            {
                var candles = await _tinkoffService.GetCandlesAsync(stock, KnownTimeframes.Daily);
                int result = await _storageService.SaveCandlesAsync(
                    $"{stock.Ticker}_{KnownTimeframes.Daily}", candles);
            }            
        }
    }
}