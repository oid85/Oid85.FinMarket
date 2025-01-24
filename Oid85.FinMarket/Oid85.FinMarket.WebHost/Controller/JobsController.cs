using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/[controller]")]
[ApiController]
public class JobsController(
    IJobService jobService) 
    : FinMarketBaseController
{
    /// <summary>
    /// Запустить load-instruments
    /// </summary>
    [HttpGet("run/load-instruments")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadInstrumentsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            jobService.LoadInstrumentsAsync);
    
    /// <summary>
    /// Запустить load-prices
    /// </summary>
    [HttpGet("run/load-prices")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadLastPricesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            jobService.LoadLastPricesAsync);
    
    /// <summary>
    /// Запустить load-bond-coupons
    /// </summary>
    [HttpGet("run/load-bond-coupons")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondCouponsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            jobService.LoadBondCouponsAsync);
    
    /// <summary>
    /// Запустить load-dividend-infos
    /// </summary>
    [HttpGet("run/load-dividend-infos")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadDividendInfosAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            jobService.LoadDividendInfosAsync);
    
    /// <summary>
    /// Запустить load-asset-fundamentals
    /// </summary>
    [HttpGet("run/load-asset-fundamentals")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadAssetFundamentalsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            jobService.LoadAssetFundamentalsAsync);
    
    /// <summary>
    /// Запустить load-forecasts
    /// </summary>
    [HttpGet("run/load-forecasts")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadForecastsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            jobService.LoadForecastsAsync);
    
    /// <summary>
    /// Запустить load-candles
    /// </summary>
    [HttpGet("run/load-candles")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadCandlesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            jobService.LoadCandlesAsync);
    
    /// <summary>
    /// Запустить calculate-spreads
    /// </summary>
    [HttpGet("run/calculate-spreads")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> CalculateSpreadsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            jobService.CalculateSpreadsAsync);
    
    /// <summary>
    /// Запустить calculate-multiplicators
    /// </summary>
    [HttpGet("run/calculate-multiplicators")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> CalculateMultiplicatorsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            jobService.CalculateMultiplicatorsAsync);
    
    /// <summary>
    /// Запустить analyse
    /// </summary>
    [HttpGet("run/analyse")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> AnalyseAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            jobService.AnalyseAsync);
    
    /// <summary>
    /// Запустить check-market-events
    /// </summary>
    [HttpGet("run/check-market-events")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> CheckMarketEventsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            jobService.CheckMarketEventsAsync);
    
    /// <summary>
    /// Запустить send-notifications
    /// </summary>
    [HttpGet("run/send-notifications")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> SendNotificationsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            jobService.SendNotificationsAsync);
}