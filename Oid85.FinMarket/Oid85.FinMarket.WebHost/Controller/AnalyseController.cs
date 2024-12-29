using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api")]
[ApiController]
public class AnalyseController(
    IAnalyseService analyseService) 
    : FinMarketBaseController
{
    /// <summary>
    /// Выполнить анализ акций
    /// </summary>
    [HttpGet("analyse/stocks")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> AnalyseStocksAsync() =>
        GetResponseAsync(
            analyseService.AnalyseStocksAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });   
    
    /// <summary>
    /// Выполнить анализ индексов
    /// </summary>
    [HttpGet("analyse/indexes")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> AnalyseIndexesAsync() =>
        GetResponseAsync(
            analyseService.AnalyseIndexesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            }); 
}