using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller
{
    [Route("api")]
    [ApiController]
    public class LoadController(ILoadService loadService) : FinMarketBaseController
    {
        /// <summary>
        /// Загрузить справочник акций
        /// </summary>
        [HttpGet("load-stocks")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> LoadStocksAsync() =>
            GetResponseAsync<object, BaseResponse<object>>(
                loadService.LoadStocksAsync);

        /// <summary>
        /// Загрузить справочник облигаций
        /// </summary>
        [HttpGet("load-bonds")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> LoadBondsAsync() =>
            GetResponseAsync<object, BaseResponse<object>>(
                loadService.LoadBondsAsync);
        
        /// <summary>
        /// Подгрузить последние свечи
        /// </summary>
        [HttpGet("load-stocks-daily-candles")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> LoadStocksDailyCandlesAsync() =>
            GetResponseAsync<object, BaseResponse<object>>(
                loadService.LoadCandlesAsync);

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
                () => loadService.LoadCandlesAsync(year));

        /// <summary>
        /// Подгрузить данные о дивидендах
        /// </summary>
        [HttpGet("load-dividends-info")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> LoadDividendInfosAsync() =>
            GetResponseAsync<object, BaseResponse<object>>(
                loadService.LoadDividendInfosAsync);
    }    
}
