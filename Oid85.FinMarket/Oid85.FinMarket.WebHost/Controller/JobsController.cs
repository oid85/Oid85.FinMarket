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
    /// Загрузка данных и расчет
    /// </summary>
    [HttpGet("run/load-and-calculate")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadInstrumentsAsync() =>
        GetResponseAsync(
            jobService.LoadAndCalculate,
            result => new BaseResponse<bool>
            {
                Result = result
            });
}