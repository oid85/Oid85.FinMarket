using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/[controller]")]
[ApiController]
public class FuturesController(
    ILoadService loadService,
    IAnalyseService analyseService,
    IReportService reportService,
    ISpreadService spreadService, 
    IFutureRepository futureRepository) 
    : FinMarketBaseController
{
    /// <summary>
    /// Получить тикеры фьючерсов из листа наблюдения
    /// </summary>
    [HttpGet("watch-list-tickers")]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFuturesWatchListAsync() =>
        await GetResponseAsync(
            async () => (await futureRepository.GetWatchListAsync())
                .Select(x => x.Ticker)
                .ToList(),
            result => new BaseResponse<List<string>>
            {
                Result = result
            });
    
    /// <summary>
    /// Загрузить справочник фьючерсов
    /// </summary>
    [HttpGet("load-catalog")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadFuturesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadFuturesAsync);
        
    /// <summary>
    /// Подгрузить свечи по фьючерсов
    /// </summary>
    [HttpGet("load-daily-candles")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadFutureDailyCandlesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadFutureDailyCandlesAsync);
    
    /// <summary>
    /// Загрузить последние цены фьючерсов
    /// </summary>
    [HttpGet("load-last-prices")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadFutureLastPricesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadFutureLastPricesAsync);
    
    /// <summary>
    /// Расчитать спреды
    /// </summary>
    [HttpGet("spreads")]
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
    
    /// <summary>
    /// Выполнить анализ фьючерсов
    /// </summary>
    [HttpGet("analyse")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> AnalyseFuturesAsync() =>
        GetResponseAsync(
            analyseService.AnalyseFuturesAsync,
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
    public Task<IActionResult> ReportFuturesAnalyseSupertrendAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportFuturesAnalyseSupertrendAsync(request),
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
    public Task<IActionResult> ReportFuturesAnalyseCandleSequenceAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportFuturesAnalyseCandleSequenceAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет по анализу Растущий объем
    /// </summary>        
    [HttpPost("report/analyse-candle-volume")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportFuturesAnalyseCandleVolumeAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportFuturesAnalyseCandleVolumeAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет по анализу Rsi
    /// </summary>        
    [HttpPost("report/analyse-rsi")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportFuturesAnalyseRsiAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportFuturesAnalyseRsiAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет по спредам
    /// </summary>        
    [HttpGet("report/spreads")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportSpreadsAsync() =>
        GetResponseAsync(
            () => reportService.GetReportSpreadsAsync(),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
}