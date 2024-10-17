using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Application.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Storage;
using Oid85.FinMarket.External.Tinkoff;
using Oid85.FinMarket.WebHost.Controller.Base;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.WebHost.Controller
{
    [Route("api")]
    [ApiController]
    public class LoadController : FinMarketBaseController
    {
        private readonly ILoadService _loadService;

        public LoadController(
            ILoadService loadService)
        {
            _loadService = loadService;
        }

        /// <summary>
        /// Загрузить справочник акций
        /// </summary>
        [HttpGet("load-stocks-catalog")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> LoadStocksCatalogAsync() =>
            GetResponseAsync<object, BaseResponse<object>>(
                _loadService.LoadStocksCatalogAsync);

        /// <summary>
        /// Загрузить справочник облигаций
        /// </summary>
        [HttpGet("load-bonds-catalog")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> LoadBondsCatalogAsync() =>
            GetResponseAsync<object, BaseResponse<object>>(
                _loadService.LoadBondsCatalogAsync);

        /// <summary>
        /// Загрузить справочник фьючерсов
        /// </summary>
        [HttpGet("load-futures-catalog")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> LoadFuturesCatalogAsync() =>
            GetResponseAsync<object, BaseResponse<object>>(
                _loadService.LoadFuturesCatalogAsync);

        /// <summary>
        /// Загрузить справочник валют
        /// </summary>
        [HttpGet("load-currencies-catalog")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> LoadCurrenciesCatalogAsync() =>
            GetResponseAsync<object, BaseResponse<object>>(
                _loadService.LoadCurrenciesCatalogAsync);

        /// <summary>
        /// Подгрузить последние свечи
        /// </summary>
        [HttpGet("load-stocks-daily-candles")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> LoadStocksDailyCandlesAsync() =>
            GetResponseAsync<object, BaseResponse<object>>(
                _loadService.LoadStocksDailyCandlesAsync);

        /// <summary>
        /// Загрузить свечи за конкретный год
        /// </summary>
        /// <param name="year">Год</param>
        [HttpGet("load-stocks-daily-candles-for-year/{year}")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> LoadStocksDailyCandlesForYearAsync(int year) =>
            GetResponseAsync<object, BaseResponse<object>>(
                () => _loadService.LoadStocksDailyCandlesForYearAsync(year));
    }    
}
