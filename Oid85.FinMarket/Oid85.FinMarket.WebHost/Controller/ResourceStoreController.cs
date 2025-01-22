using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/[controller]")]
[ApiController]
public class ResourceStoreController(
    IResourceStoreService resourceStoreService) 
    : FinMarketBaseController
{
    /// <summary>
    /// Получить тикеры акций из списка наблюдения
    /// </summary>
    [HttpGet("watch-lists/shares")]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetSharesWatchlistAsync() =>
        GetResponseAsync(
            resourceStoreService.GetSharesWatchlistAsync,
            result => new BaseResponse<List<string>>
            {
                Result = result
            });
    
    /// <summary>
    /// Получить тикеры облигаций из списка наблюдения
    /// </summary>
    [HttpGet("watch-lists/bonds")]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetBondsWatchlistAsync() =>
        GetResponseAsync(
            resourceStoreService.GetBondsWatchlistAsync,
            result => new BaseResponse<List<string>>
            {
                Result = result
            });
    
    /// <summary>
    /// Получить тикеры фьючерсов из списка наблюдения
    /// </summary>
    [HttpGet("watch-lists/futures")]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetFuturesWatchlistAsync() =>
        GetResponseAsync(
            resourceStoreService.GetFuturesWatchlistAsync,
            result => new BaseResponse<List<string>>
            {
                Result = result
            });
    
    /// <summary>
    /// Получить тикеры валют из списка наблюдения
    /// </summary>
    [HttpGet("watch-lists/currencies")]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetCurrenciesWatchlistAsync() =>
        GetResponseAsync(
            resourceStoreService.GetCurrenciesWatchlistAsync,
            result => new BaseResponse<List<string>>
            {
                Result = result
            });
    
    /// <summary>
    /// Получить тикеры индексов из списка наблюдения
    /// </summary>
    [HttpGet("watch-lists/indexes")]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetIndexesWatchlistAsync() =>
        GetResponseAsync(
            resourceStoreService.GetIndexesWatchlistAsync,
            result => new BaseResponse<List<string>>
            {
                Result = result
            });
}