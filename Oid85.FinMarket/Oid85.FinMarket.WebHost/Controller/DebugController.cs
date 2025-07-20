using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Application.Services.AnalyseServices;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/debug")]
[ApiController]
public class DebugController(IStatisticalArbitrationService service) 
    : FinMarketBaseController
{
    [HttpGet("debug")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Debug()
    {
        await service.CalculateCorrelationAsync();
            
        return Ok();
    }
}