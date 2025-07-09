using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Application.Services.AnalyseServices;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/debug")]
[ApiController]
public class DebugController(
    ChannelMessageQueue channelMessageQueue) 
    : FinMarketBaseController
{
    [HttpGet("debug1")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Debug1()
    {
        await channelMessageQueue.WriteAsync();
        
        return Ok();
    }
    
    [HttpGet("debug2")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Debug2()
    {
        await channelMessageQueue.ReadAsync();
        
        return Ok();
    }
}