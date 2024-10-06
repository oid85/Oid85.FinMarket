using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Storage;
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

        [HttpGet("analyse-stocks")]
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
