﻿using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.BacktestResults;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/algo")]
[ApiController]
public class AlgoController(
    IAlgoService algoService,
    IAlgoReportService reportService)
    : FinMarketBaseController
{
    /// <summary>
    /// Выполнить оптимизацию
    /// </summary>
    [HttpGet("optimization")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> RunOptimizationAsync() =>
        GetResponseAsync(
            algoService.OptimizeAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });

    /// <summary>
    /// Выполнить бэктест
    /// </summary>
    [HttpGet("backtest")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> RunBacktestAsync() =>
        GetResponseAsync(
            algoService.BacktestAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });

    /// <summary>
    /// Рассчитать сигналы
    /// </summary>
    [HttpGet("calculate-strategy-signals")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> CalculateStrategySignalsAsync() =>
        GetResponseAsync(
            algoService.CalculateStrategySignalsAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });    
    
    /// <summary>
    /// Получить результаты бэктеста
    /// </summary>
    [HttpPost("backtest-results")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetBacktestResultsAsync([FromBody] TickerStrategyRequest request) =>
        GetResponseAsync(
            () => reportService.GetBacktestResultsAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });

    /// <summary>
    /// Получить бэктест по id
    /// </summary>
    [HttpPost("backtest-result-by-id")]
    [ProducesResponseType(typeof(BaseResponse<BacktestResultData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<BacktestResultData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<BacktestResultData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetBacktestResultByIdAsync(
        [FromBody] IdRequest request) =>
        GetResponseAsync(
            () => reportService.GetBacktestResultByIdAsync(request),
            result => new BaseResponse<BacktestResultData>
            {
                Result = result
            });    
    
    /// <summary>
    /// Получить бэктест по тикеру
    /// </summary>
    [HttpPost("backtest-result-by-ticker")]
    [ProducesResponseType(typeof(BaseResponse<BacktestResultData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<BacktestResultData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<BacktestResultData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetBacktestResultByTickerAsync(
        [FromBody] TickerRequest request) =>
        GetResponseAsync(
            () => reportService.GetBacktestResultByTickerAsync(request),
            result => new BaseResponse<BacktestResultData>
            {
                Result = result
            });     
    
    /// <summary>
    /// Получить бэктест портфеля
    /// </summary>
    [HttpPost("backtest-result-portfolio")]
    [ProducesResponseType(typeof(BaseResponse<BacktestResultData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<BacktestResultData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<BacktestResultData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetBacktestResultPortfolioAsync() =>
        GetResponseAsync(
            () => reportService.GetBacktestResultPortfolioAsync(),
            result => new BaseResponse<BacktestResultData>
            {
                Result = result
            });     
    
    /// <summary>
    /// Получить сигналы стратегий
    /// </summary>
    [HttpPost("strategy-signals")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetStrategySignalsAsync() =>
        GetResponseAsync(
            reportService.GetStrategySignalsAsync,
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
}