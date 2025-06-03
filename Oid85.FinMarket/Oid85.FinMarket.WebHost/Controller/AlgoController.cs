using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.DiagramServices;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/algo")]
[ApiController]
public class AlgoController(
    IBacktestService backtestService,
    IOptimizationService optimizationService) 
    : FinMarketBaseController
{
    /// <summary>
    /// Выполнить оптимизацию
    /// </summary>
    [HttpPost("optimization")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> RunOptimizationAsync() =>
        GetResponseAsync(
            optimizationService.OptimizeAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Выполнить бэктест
    /// </summary>
    [HttpPost("backtest")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> RunBacktestAsync() =>
        GetResponseAsync(
            backtestService.BacktestAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });    
}