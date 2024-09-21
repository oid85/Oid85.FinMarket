using ILogger = NLog.ILogger;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Tinkoff;

namespace Oid85.FinMarket.WebHost.HostedServices
{
    public class InitHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ITinkoffService _tinkoffService;
        private readonly ICatalogService _catalogService;

        public InitHostedService(
            ILogger logger,
            IConfiguration configuration,
            ITinkoffService tinkoffService,
            ICatalogService catalogService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _tinkoffService = tinkoffService ?? throw new ArgumentNullException(nameof(tinkoffService));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await UptateStocksCatalog();
            await UptateBondsCatalog();
            await UptateFuturesCatalog();
            await UptateCurrenciesCatalog();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task UptateStocksCatalog()
        {
            try
            {
                var stocks = _tinkoffService.GetStocks();

                await _catalogService.LoadFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Stocks, stocks);

                _logger.Info($"Обновлен каталог акций");
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        private async Task UptateBondsCatalog()
        {
            try
            {
                var bonds = _tinkoffService.GetBonds();

                await _catalogService.LoadFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Bonds, bonds);

                _logger.Info($"Обновлен каталог облигаций");
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        private async Task UptateFuturesCatalog()
        {
            try
            {
                var futures = _tinkoffService.GetFutures();

                await _catalogService.LoadFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Futures, futures);

                _logger.Info($"Обновлен каталог фьючерсов");
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        private async Task UptateCurrenciesCatalog()
        {
            try
            {
                var currencies = _tinkoffService.GetCurrencies();

                await _catalogService.LoadFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Currencies, currencies);

                _logger.Info($"Обновлен каталог валют");
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }
    }
}