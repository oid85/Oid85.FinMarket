using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/[controller]")]
[ApiController]
public class DebugController(
    ITickerListUtilService tickerListUtilService) 
    : FinMarketBaseController
{
    /// <summary>
    /// Отправить сообщение в телеграм
    /// </summary>
    [HttpGet("debug")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Debug()
    {
        await tickerListUtilService.GetFuturesByTickerListAsync(KnownTickerLists.FuturesWatchlist);
        
        return Ok();
    }
}