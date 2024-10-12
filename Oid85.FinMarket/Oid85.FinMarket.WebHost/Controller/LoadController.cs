using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Models.Responses;
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
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadStocksCatalogAsync()
        {
            _logger.Trace($"Request - /api/load-stocks-catalog");

            try
            {
                var stocks = _tinkoffService.GetStocks();

                await _catalogService.UpdateFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Stocks, stocks);

                var response = new CommonResponse<string>("OK");

                return Ok(response);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);

                var error = new ResponseError()
                {
                    ErrorCode = 500,
                    ErrorDescription = "Ошибка при выполнении запроса",
                    ErrorMessage = exception.Message
                };

                var response = new CommonResponse<string>(error);

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Загрузить справочник облигаций
        /// </summary>
        [HttpGet("load-bonds-catalog")]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadBondsCatalogAsync()
        {
            _logger.Trace($"Request - /api/load-bonds-catalog");

            try
            {
                var bonds = _tinkoffService.GetBonds();

                await _catalogService.UpdateFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Bonds, bonds);

                var response = new CommonResponse<string>("OK");

                return Ok(response);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);

                var error = new ResponseError()
                {
                    ErrorCode = 500,
                    ErrorDescription = "Ошибка при выполнении запроса",
                    ErrorMessage = exception.Message
                };

                var response = new CommonResponse<string>(error);

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Загрузить справочник фьючерсов
        /// </summary>
        [HttpGet("load-futures-catalog")]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadFuturesCatalogAsync()
        {
            _logger.Trace($"Request - /api/load-futures-catalog");

            try
            {
                var futures = _tinkoffService.GetFutures();

                await _catalogService.UpdateFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Futures, futures);

                var response = new CommonResponse<string>("OK");

                return Ok(response);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);

                var error = new ResponseError()
                {
                    ErrorCode = 500,
                    ErrorDescription = "Ошибка при выполнении запроса",
                    ErrorMessage = exception.Message
                };

                var response = new CommonResponse<string>(error);

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Загрузить справочник валют
        /// </summary>
        [HttpGet("load-currencies-catalog")]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadCurrenciesCatalogAsync()
        {
            _logger.Trace($"Request - /api/load-currencies-catalog");

            try
            {
                var currencies = _tinkoffService.GetCurrencies();

                await _catalogService.UpdateFinancicalInstrumentsAsync(
                    KnownFinancicalInstrumentTypes.Currencies, currencies);

                var response = new CommonResponse<string>("OK");

                return Ok(response);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);

                var error = new ResponseError()
                {
                    ErrorCode = 500,
                    ErrorDescription = "Ошибка при выполнении запроса",
                    ErrorMessage = exception.Message
                };

                var response = new CommonResponse<string>(error);

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Подгрузить последние свечи
        /// </summary>
        [HttpGet("load-stocks-daily-candles")]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadStocksDailyCandlesAsync()
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

                var response = new CommonResponse<string>("OK");

                return Ok(response);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);

                var error = new ResponseError()
                {
                    ErrorCode = 500,
                    ErrorDescription = "Ошибка при выполнении запроса",
                    ErrorMessage = exception.Message
                };

                var response = new CommonResponse<string>(error);

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Загрузить свечи за конкретный год
        /// </summary>
        /// <param name="year">Год</param>
        [HttpGet("load-stocks-daily-candles-for-year/{year}")]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadStocksDailyCandlesForYearAsync(int year)
        {
            _logger.Trace($"Request - /api/load-stocks-daily-candles-for-year");

            try
            {
                var stocks = await _catalogService
                    .GetActiveFinancicalInstrumentsAsync(KnownFinancicalInstrumentTypes.Stocks);

                var data = new List<Tuple<string, List<Candle>>>();

                foreach (var stock in stocks)
                {
                    _logger.Trace($"Load '{stock.Ticker}'");
                    var candles = await _tinkoffService.GetCandlesAsync(stock, KnownTimeframes.Daily, year);
                    data.Add(new Tuple<string, List<Candle>>($"{stock.Ticker}_{KnownTimeframes.Daily}", candles));
                }

                await _storageService.SaveCandlesAsync(data);

                var response = new CommonResponse<string>("OK");

                return Ok(response);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);

                var error = new ResponseError()
                {
                    ErrorCode = 500,
                    ErrorDescription = "Ошибка при выполнении запроса",
                    ErrorMessage = exception.Message
                };

                var response = new CommonResponse<string>(error);

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }    
}
