using NLog;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Storage;
using Oid85.FinMarket.External.Tinkoff;

namespace Oid85.FinMarket.Application.Services
{
    public class LoadService : ILoadService
    {
        private readonly ILogger _logger;
        private readonly ITinkoffService _tinkoffService;
        private readonly ICatalogService _catalogService;
        private readonly IStorageService _storageService;

        public LoadService(
            ILogger logger,
            ITinkoffService tinkoffService,
            ICatalogService catalogService,
            IStorageService storageService)
        {
            _logger = logger;
            _tinkoffService = tinkoffService;
            _catalogService = catalogService;
            _storageService = storageService;
        }

        public async Task LoadBondsCatalogAsync()
        {
            var bonds = await _tinkoffService.GetBondsAsync();

            await _catalogService.UpdateFinInstrumentsAsync(
                KnownFinInstrumentTypes.Bonds, bonds);
        }

        public async Task LoadCurrenciesCatalogAsync()
        {
            var currencies = await _tinkoffService.GetCurrenciesAsync();

            await _catalogService.UpdateFinInstrumentsAsync(
                KnownFinInstrumentTypes.Currencies, currencies);
        }

        public async Task LoadFuturesCatalogAsync()
        {
            var futures = await _tinkoffService.GetFuturesAsync();

            await _catalogService.UpdateFinInstrumentsAsync(
                KnownFinInstrumentTypes.Futures, futures);
        }

        public async Task LoadStocksCatalogAsync()
        {
            var stocks = await _tinkoffService.GetStocksAsync();

            await _catalogService.UpdateFinInstrumentsAsync(
                KnownFinInstrumentTypes.Stocks, stocks);
        }

        public async Task LoadStocksDailyCandlesAsync()
        {
            var stocks = await _catalogService
                .GetActiveFinInstrumentsAsync(KnownFinInstrumentTypes.Stocks);

            var data = new List<Tuple<string, List<Candle>>>();

            foreach (var stock in stocks)
            {
                var candles = await _tinkoffService.GetCandlesAsync(stock, KnownTimeframes.Daily);
                data.Add(new Tuple<string, List<Candle>>($"{stock.Ticker}_{KnownTimeframes.Daily}", candles));
            }

            await _storageService.SaveCandlesAsync(data);
        }

        public async Task LoadStocksDailyCandlesForYearAsync(int year)
        {
            var stocks = await _catalogService
                .GetActiveFinInstrumentsAsync(KnownFinInstrumentTypes.Stocks);

            var data = new List<Tuple<string, List<Candle>>>();

            foreach (var stock in stocks)
            {
                _logger.Trace($"Load candles '{stock.Ticker}'");
                
                var candles = await _tinkoffService.GetCandlesAsync(stock, KnownTimeframes.Daily, year);
                data.Add(new Tuple<string, List<Candle>>($"{stock.Ticker}_{KnownTimeframes.Daily}", candles));
            }

            await _storageService.SaveCandlesAsync(data);
        }

        public async Task LoadDividendInfosAsync()
        {
            var stocks = await _tinkoffService.GetStocksAsync();
            var dividendInfos = await _tinkoffService.GetDividendInfoAsync(stocks);
            await _catalogService.UpdateDividendInfosAsync(dividendInfos);
        }
    }
}
