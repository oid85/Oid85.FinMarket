using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Storage;
using Oid85.FinMarket.External.Tinkoff;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.WebHost.Controller
{
    [Route("api")]
    [ApiController]
    public class LoadController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ITinkoffService _tinkoffService;
        private readonly ICatalogService _catalogService;
        private readonly IStorageService _storageService;

        public LoadController(
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

        /// <summary>
        /// Загрузить справочник акций
        /// </summary>
        [HttpGet("load-stocks-catalog")]
        public async Task LoadStocksCatalogAsync()
        {
            _logger.Trace($"Request - /api/load-stocks-catalog");

            try
            {
                var stocks = _tinkoffService.GetStocks();

                await _catalogService.LoadFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Stocks, stocks);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        /// <summary>
        /// Загрузить справочник облигаций
        /// </summary>
        [HttpGet("load-bonds-catalog")]
        public async Task LoadBondsCatalogAsync()
        {
            _logger.Trace($"Request - /api/load-bonds-catalog");

            try
            {
                var bonds = _tinkoffService.GetBonds();

                await _catalogService.LoadFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Bonds, bonds);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        /// <summary>
        /// Загрузить справочник фьючерсов
        /// </summary>
        [HttpGet("load-futures-catalog")]
        public async Task LoadFuturesCatalogAsync()
        {
            _logger.Trace($"Request - /api/load-futures-catalog");

            try
            {
                var futures = _tinkoffService.GetFutures();

                await _catalogService.LoadFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Futures, futures);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        /// <summary>
        /// Загрузить справочник валют
        /// </summary>
        [HttpGet("load-currencies-catalog")]
        public async Task LoadCurrenciesCatalogAsync()
        {
            _logger.Trace($"Request - /api/load-currencies-catalog");

            try
            {
                var currencies = _tinkoffService.GetCurrencies();

                await _catalogService.LoadFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Currencies, currencies);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        /// <summary>
        /// Подгрузить последние свечи
        /// </summary>
        [HttpGet("load-stocks-daily-candles")]
        public async Task LoadStocksDailyCandlesAsync()
        {
            _logger.Trace($"Request - /api/load-stocks-daily-candles");

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

        /// <summary>
        /// Загрузить свечи за конкретный год
        /// </summary>
        /// <param name="year">Год</param>
        [HttpGet("load-stocks-daily-candles-for-year/{year}")]
        public async Task LoadStocksDailyCandlesForYearAsync(int year)
        {
            _logger.Trace($"Request - /api/load-stocks-daily-candles-for-year");

            try
            {
                var stocks = await _catalogService
                    .GetActiveFinancicalInstrumentsAsync(KnownFinancicalInstrumentTypes.Stocks);

                var data = new List<Tuple<string, List<Candle>>>();

                foreach (var stock in stocks)
                {
                    var candles = await _tinkoffService.GetCandlesAsync(stock, KnownTimeframes.Daily, year);
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
