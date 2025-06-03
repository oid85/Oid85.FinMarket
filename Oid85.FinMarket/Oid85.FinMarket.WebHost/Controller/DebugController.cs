using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Application.Services.AnalyseServices;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/debug")]
[ApiController]
public class DebugController(
    DonchianAnalyseService donchianAnalyseService) 
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
        await donchianAnalyseService.DonchianAnalyseAsync(Guid.Parse("1e19953d-01c6-4ecd-a5f4-53ae3ed44029"));
        
        return Ok();
    }
}