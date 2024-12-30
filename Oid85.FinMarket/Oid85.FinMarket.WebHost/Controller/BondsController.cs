using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("[controller]/api")]
[ApiController]
public class BondsController(
    ILoadService loadService,
    IAnalyseService analyseService,
    IReportService reportService,
    IBondRepository bondRepository) 
    : FinMarketBaseController
{
    /// <summary>
    /// Получить тикеры облигаций из листа наблюдения
    /// </summary>
    [HttpGet("watch-list-tickers")]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBondsWatchListAsync() =>
        await GetResponseAsync(
            async () => (await bondRepository.GetWatchListAsync())
                .Select(x => x.Ticker)
                .ToList(),
            result => new BaseResponse<List<string>>
            {
                Result = result
            });
    
    /// <summary>
    /// Загрузить справочник облигаций
    /// </summary>
    [HttpGet("load-catalog")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadBondsAsync);
        
    /// <summary>
    /// Подгрузить свечи по облигаций
    /// </summary>
    [HttpGet("load-daily-candles")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondDailyCandlesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadBondDailyCandlesAsync);
    
    /// <summary>
    /// Загрузить последние цены облигаций
    /// </summary>
    [HttpGet("load-last-prices")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondLastPricesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadBondLastPricesAsync);
    
    /// <summary>
    /// Выполнить анализ облигаций
    /// </summary>
    [HttpGet("analyse")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> AnalyseBondsAsync() =>
        GetResponseAsync(
            analyseService.AnalyseBondsAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет по анализу Супертренд
    /// </summary>        
    [HttpPost("report/analyse-supertrend")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportBondsAnalyseSupertrendAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportBondsAnalyseSupertrendAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет по анализу Последовательность свечей одного цвета
    /// </summary>        
    [HttpPost("report/analyse-candle-sequence")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportBondsAnalyseCandleSequenceAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportBondsAnalyseCandleSequenceAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет по облигациям
    /// </summary>        
    [HttpGet("report/bonds")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportBondsAsync() =>
        GetResponseAsync(
            () => reportService.GetReportBondsAsync(),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
}