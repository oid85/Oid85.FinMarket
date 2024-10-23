using ILogger = NLog.ILogger;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Tinkoff;
using Oid85.FinMarket.External.Settings;
using Oid85.FinMarket.Application.Services;

namespace Oid85.FinMarket.WebHost.HostedServices
{
    public class InitHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly ITinkoffService _tinkoffService;
        private readonly ICatalogService _catalogService;
        private readonly ISettingsService _settingsService;
        private readonly ILoadService _loadService;

        public InitHostedService(
            ILogger logger,
            ITinkoffService tinkoffService,
            ICatalogService catalogService,
            ISettingsService settingsService,
            ILoadService loadService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tinkoffService = tinkoffService ?? throw new ArgumentNullException(nameof(tinkoffService));
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var updateEnable = await _settingsService
                .GetBoolValueAsync(KnownSettingsKeys.UpdateFinancicalInstrumentsOnStart_Enable);

            if (updateEnable) 
            {
                await UpdateStocksCatalogAsync();
                await UpdateBondsCatalogAsync();
                await UpdateFuturesCatalogAsync();
                await UpdateCurrenciesCatalogAsync();
                await UpdateDividendInfoAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task UpdateStocksCatalogAsync()
        {
            try
            {
                var stocks = await _tinkoffService.GetStocksAsync();

                await _catalogService.UpdateFinInstrumentsAsync(
                    KnownFinInstrumentTypes.Stocks, stocks);

                _logger.Info($"Обновлен каталог акций");
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        private async Task UpdateBondsCatalogAsync()
        {
            try
            {
                var bonds = await _tinkoffService.GetBondsAsync();

                await _catalogService.UpdateFinInstrumentsAsync(
                    KnownFinInstrumentTypes.Bonds, bonds);

                _logger.Info($"Обновлен каталог облигаций");
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        private async Task UpdateFuturesCatalogAsync()
        {
            try
            {
                var futures = await _tinkoffService.GetFuturesAsync();

                await _catalogService.UpdateFinInstrumentsAsync(
                    KnownFinInstrumentTypes.Futures, futures);

                _logger.Info($"Обновлен каталог фьючерсов");
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        private async Task UpdateCurrenciesCatalogAsync()
        {
            try
            {
                var currencies = await _tinkoffService.GetCurrenciesAsync();

                await _catalogService.UpdateFinInstrumentsAsync(
                    KnownFinInstrumentTypes.Currencies, currencies);

                _logger.Info($"Обновлен каталог валют");
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        private async Task UpdateDividendInfoAsync()
        {
            try
            {
                await _loadService.LoadDividendInfosAsync();

                _logger.Info($"Обновлена информация по дивидендам");
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }
    }
}