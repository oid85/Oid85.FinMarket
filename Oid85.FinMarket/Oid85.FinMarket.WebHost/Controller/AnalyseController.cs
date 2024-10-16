using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Application.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Catalogs;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.WebHost.Controller
{
    [Route("api")]
    [ApiController]
    public class AnalyseController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICatalogService _catalogService;
        private readonly IAnalyseService _analyseService;

        public AnalyseController(
            ILogger logger,
            ICatalogService catalogService,
            IAnalyseService analyseService)
        {
            _logger = logger;
            _catalogService = catalogService;
            _analyseService = analyseService;
        }

        /// <summary>
        /// Анализ при помощи индикатора Супертренд
        /// </summary>
        [HttpGet("analyse-stocks")]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CommonResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AnalyseStocksAsync()
        {
            _logger.Trace($"Request - /api/analyse-stocks");

            try
            {
                var stocks = await _catalogService
                    .GetActiveFinInstrumentsAsync(KnownFinInstrumentTypes.Stocks);

                foreach (var stock in stocks)
                {
                    _logger.Trace($"Analyse '{stock.Ticker}'");

                    await _analyseService.SupertrendAnalyseAsync(stock, KnownTimeframes.Daily);
                    await _analyseService.CandleSequenceAnalyseAsync(stock, KnownTimeframes.Daily);
                }

                var response = new CommonResponse<string>("OK");

                return Ok(response);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);

                var error = new ResponseError()
                {
                    Code = 500,
                    Description = "Ошибка при выполнении запроса",
                    Message = exception.Message
                };

                var response = new CommonResponse<string>(error);

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
