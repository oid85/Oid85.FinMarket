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

[Route("api/[controller]")]
[ApiController]
public class IndexesController(
    ILoadService loadService,
    IAnalyseService analyseService,
    IIndexesReportService reportService,
    IIndexesDiagramService diagramService,
    ITickerListUtilService tickerListUtilService) 
    : FinMarketBaseController
{
    /// <summary>
    /// Получить тикеры индексов из листа наблюдения
    /// </summary>
    [HttpGet("watch-list-tickers")]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetIndexesWatchListAsync() =>
        await GetResponseAsync(
            async () => (await tickerListUtilService.GetFinIndexesByTickerListAsync(KnownTickerLists.IndexesWatchlist))
                .Select(x => x.Ticker)
                .ToList(),
            result => new BaseResponse<List<string>>
            {
                Result = result
            });
    
    /// <summary>
    /// Загрузить справочник индексов
    /// </summary>
    [HttpGet("load-catalog")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadIndexesAsync() =>
        GetResponseAsync(
            loadService.LoadIndexesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
        
    /// <summary>
    /// Подгрузить свечи по индексов
    /// </summary>
    [HttpGet("load-daily-candles")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadIndexDailyCandlesAsync() =>
        GetResponseAsync(
            loadService.LoadIndexDailyCandlesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Загрузить последние цены индексов
    /// </summary>
    [HttpGet("load-last-prices")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadIndexLastPricesAsync() =>
        GetResponseAsync(
            loadService.LoadIndexLastPricesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Выполнить анализ индексов
    /// </summary>
    [HttpGet("daily-analyse")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> DailyAnalyseIndexesAsync() =>
        GetResponseAsync(
            analyseService.DailyAnalyseIndexesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Сводный анализ
    /// </summary>
    [HttpPost("report/aggregated-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetAggregatedAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetAggregatedAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Супертренд
    /// </summary>
    [HttpPost("report/supertrend-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetSupertrendAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetSupertrendAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Последовательность свечей одного цвета
    /// </summary>
    [HttpPost("report/candle-sequence-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetCandleSequenceAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetCandleSequenceAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет RSI
    /// </summary>
    [HttpPost("report/rsi-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetRsiAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetRsiAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Доходность LTM
    /// </summary>
    [HttpPost("report/yield-ltm-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetYieldLtmAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetYieldLtmAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Максимальная просадка от максимума
    /// </summary>
    [HttpPost("report/drawdown-from-maximum-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetDrawdownFromMaximumAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetDrawdownFromMaximumAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            }); 
    
    /// <summary>
    /// Отчет Рыночные события
    /// </summary>
    [HttpPost("report/active-market-events-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetActiveMarketEventsAnalyseAsync(
        [FromBody] TickerListRequest request) =>
        GetResponseAsync(
            () => reportService.GetActiveMarketEventsAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });    
    
    /// <summary>
    /// Диаграмма График цен закрытия (дневные)
    /// </summary>
    [HttpPost("diagram/daily-close-prices")]
    [ProducesResponseType(typeof(BaseResponse<SimpleDiagramData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<SimpleDiagramData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<SimpleDiagramData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetDailyClosePricesAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => diagramService.GetDailyClosePricesAsync(request),
            result => new BaseResponse<SimpleDiagramData>
            {
                Result = result
            });
}