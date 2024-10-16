using ILogger = NLog.ILogger;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Tinkoff;
using Oid85.FinMarket.External.Settings;

namespace Oid85.FinMarket.WebHost.HostedServices
{
    public class InitHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly ITinkoffService _tinkoffService;
        private readonly ICatalogService _catalogService;
        private readonly ISettingsService _settingsService;

        public InitHostedService(
            ILogger logger,
            ITinkoffService tinkoffService,
            ICatalogService catalogService,
            ISettingsService settingsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tinkoffService = tinkoffService ?? throw new ArgumentNullException(nameof(tinkoffService));
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var updateEnable = await _settingsService
                .GetBoolValueAsync(KnownSettingsKeys.UpdateFinancicalInstrumentsOnStart_Enable);

            if (updateEnable) 
            {
                await UpdateStocksCatalog();
                await UpdateBondsCatalog();
                await UpdateFuturesCatalog();
                await UpdateCurrenciesCatalog();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task UpdateStocksCatalog()
        {
            try
            {
                var stocks = _tinkoffService.GetStocks();

                await _catalogService.UpdateFinInstrumentsAsync(
                    KnownFinInstrumentTypes.Stocks, stocks);

                _logger.Info($"Обновлен каталог акций");
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        private async Task UpdateBondsCatalog()
        {
            try
            {
                var bonds = _tinkoffService.GetBonds();

                await _catalogService.UpdateFinInstrumentsAsync(
                    KnownFinInstrumentTypes.Bonds, bonds);

                _logger.Info($"Обновлен каталог облигаций");
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        private async Task UpdateFuturesCatalog()
        {
            try
            {
                var futures = _tinkoffService.GetFutures();

                await _catalogService.UpdateFinInstrumentsAsync(
                    KnownFinInstrumentTypes.Futures, futures);

                _logger.Info($"Обновлен каталог фьючерсов");
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        private async Task UpdateCurrenciesCatalog()
        {
            try
            {
                var currencies = _tinkoffService.GetCurrencies();

                await _catalogService.UpdateFinInstrumentsAsync(
                    KnownFinInstrumentTypes.Currencies, currencies);

                _logger.Info($"Обновлен каталог валют");
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }
    }
}