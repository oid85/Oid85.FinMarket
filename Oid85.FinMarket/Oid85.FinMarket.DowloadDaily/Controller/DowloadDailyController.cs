using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Tinkoff;
using ILogger = NLog.ILogger;

namespace DaGroup.WPAnalyst.DataLake.WebHost.Controllers
{
    [Route("api")]
    [ApiController]
    public class DowloadDailyController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ITinkoffService _tinkoffService;
        private readonly ICatalogService _catalogService;

        public DowloadDailyController(
            ILogger logger,
            ITinkoffService tinkoffService,
            ICatalogService catalogService)
        {
            _logger = logger;
            _tinkoffService = tinkoffService;
            _catalogService = catalogService;
        }

        [HttpGet("get-candles")]
        public async Task GetCandlesAsync()
        {
            _logger.Trace($"Request - /api/download-daily");
            
            try
            {
                var instrument = new FinancicalInstrument() 
                { 
                    Ticker = "SBER",
                    Figi = "BBG004730N88"
                };

                var candles = await _tinkoffService.GetCandlesAsync(instrument, KnownTimeframes.Daily);
            }
            
            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        [HttpGet("fill-stocks-catalog")]
        public async Task FillStocksCatalogAsync()
        {
            _logger.Trace($"Request - /api/fill-stocks-catalog");

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
    }
}
