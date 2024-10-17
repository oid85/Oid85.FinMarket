using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.WebHost.Controller.Base;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.WebHost.Controller
{
    [Route("api")]
    [ApiController]
    public class FinInstrumentController : FinMarketBaseController
    {
        private readonly ICatalogService _catalogService;        

        public FinInstrumentController(
            ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        /// <summary>
        /// Получить инструменты из листа наблюдения
        /// </summary>        
        [HttpPost("fin-instrument/watch-list")]
        [ProducesResponseType(typeof(BaseResponse<List<WatchListStock>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<List<WatchListStock>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<List<WatchListStock>>), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> GetWatchListAsync() =>
            GetResponseAsync(
                _catalogService.GetWatchListStocksAsync,
                result => new BaseResponse<List<WatchListStock>>
                {
                    Result = result
                });        
    }
}
