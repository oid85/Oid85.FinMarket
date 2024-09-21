using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
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

            try
            {
                var stocks = await _catalogService
                    .GetActiveFinancicalInstrumentsAsync(KnownFinancicalInstrumentTypes.Stocks);

                var data = new List<Tuple<string, List<Candle>>>();

                foreach (var stock in stocks)
                {
                    var candles = await _tinkoffService.GetCandlesAsync(stock, KnownTimeframes.Daily);
                    data.Add(new Tuple<string, List<Candle>>($"{stock.Ticker}_{KnownTimeframes.Daily}", candles));
                }

                await _storageService.SaveCandlesAsync(data);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }
    }
}