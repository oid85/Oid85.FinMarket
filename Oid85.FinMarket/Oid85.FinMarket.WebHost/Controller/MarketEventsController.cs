﻿using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/[controller]")]
[ApiController]
public class MarketEventsController(
    IMarketEventsReportService marketEventsReportService)
    : FinMarketBaseController
{
    /// <summary>
    /// Отчет Активные рыночные события
    /// </summary>
    [HttpPost("report/active-market-events-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetActiveMarketEventsAnalyseAsync() =>
        GetResponseAsync(
            marketEventsReportService.GetActiveMarketEventsAnalyseAsync,
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
}