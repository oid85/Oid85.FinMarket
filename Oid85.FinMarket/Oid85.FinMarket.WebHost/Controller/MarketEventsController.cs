using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/[controller]")]
[ApiController]
public class MarketEventsController(
    IMarketEventService marketEventService)
    : FinMarketBaseController
{
    /// <summary>
    /// Расчет рыночного события Супертренд
    /// </summary>
    [HttpGet("check-supertrend-market-event")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> CheckSupertrendMarketEventAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            marketEventService.CheckSupertrendMarketEventAsync);
    
    /// <summary>
    /// Расчет рыночного события Растущий объем
    /// </summary>
    [HttpGet("check-candle-volume-market-event")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> CheckCandleVolumeMarketEventAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            marketEventService.CheckCandleVolumeMarketEventAsync);
    
    /// <summary>
    /// Расчет рыночного события Свечи одного цвета
    /// </summary>
    [HttpGet("check-candle-sequence-market-event")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> CheckCandleSequenceMarketEventAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            marketEventService.CheckCandleSequenceMarketEventAsync);
}