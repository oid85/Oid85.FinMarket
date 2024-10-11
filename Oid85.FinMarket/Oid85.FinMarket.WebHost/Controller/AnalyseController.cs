using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task AnalyseStocksAsync()
        {
            _logger.Trace($"Request - /api/analyse-stocks");

            try
            {
                var stocks = await _catalogService
                    .GetActiveFinancicalInstrumentsAsync(KnownFinancicalInstrumentTypes.Stocks);

                foreach (var stock in stocks)
                {
                    _logger.Trace($"Analyse '{stock.Ticker}'");
                    await _analyseService.SupertrendAnalyseAsync(stock, KnownTimeframes.Daily);
                }                
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }
    }
}
