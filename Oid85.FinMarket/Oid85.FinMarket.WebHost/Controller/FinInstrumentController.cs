using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api")]
[ApiController]
public class FinInstrumentController(
    IShareRepository shareRepository,
    ISpreadService spreadService
    ) 
    : FinMarketBaseController
{
    /// <summary>
    /// Получить акции из листа наблюдения
    /// </summary>
    [HttpGet("fin-instrument/watch-list/stocks")]
    [ProducesResponseType(typeof(BaseResponse<List<Share>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<Share>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<Share>>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetWatchListAsync() =>
        GetResponseAsync(
            shareRepository.GetWatchListAsync,
            result => new BaseResponse<List<Share>>
            {
                Result = result
            });      
    
    /// <summary>
    /// Расчитать спреды
    /// </summary>
    [HttpGet("fin-instrument/watch-list/spreads")]
    [ProducesResponseType(typeof(BaseResponse<List<Spread>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<Spread>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<Spread>>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> CalculateSpreadsAsync() =>
        GetResponseAsync(
            spreadService.CalculateSpreadsAsync,
            result => new BaseResponse<List<Spread>>
            {
                Result = result
            });  
}