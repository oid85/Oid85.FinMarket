using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/[controller]")]
[ApiController]
public class SharesController(
    ILoadService loadService,
    IAnalyseService analyseService,
    IReportService reportService,
    IShareRepository shareRepository)
    : FinMarketBaseController
{
    /// <summary>
    /// Получить тикеры акций из листа наблюдения
    /// </summary>
    [HttpGet("watch-list-tickers")]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSharesWatchListAsync() =>
        await GetResponseAsync(
            async () => (await shareRepository.GetWatchListAsync())
                .Select(x => x.Ticker)
                .ToList(),
            result => new BaseResponse<List<string>>
            {
                Result = result
            });

    /// <summary>
    /// Загрузить справочник акций
    /// </summary>
    [HttpGet("load-catalog")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadSharesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadSharesAsync);

    /// <summary>
    /// Подгрузить свечи по акциям
    /// </summary>
    [HttpGet("load-daily-candles")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadShareDailyCandlesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadShareDailyCandlesAsync);

    /// <summary>
    /// Загрузить данные о дивидендах
    /// </summary>
    [HttpGet("load-dividends")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadDividendInfosAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadDividendInfosAsync);

    /// <summary>
    /// Загрузить фундаментальные данные по акциям
    /// </summary>
    [HttpGet("load-asset-fundamentals")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadAssetFundamentalsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadAssetFundamentalsAsync);

    /// <summary>
    /// Загрузить последние цены акций
    /// </summary>
    [HttpGet("load-last-prices")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadShareLastPricesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadShareLastPricesAsync);

    /// <summary>
    /// Выполнить анализ акций
    /// </summary>
    [HttpGet("analyse")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> AnalyseSharesAsync() =>
        GetResponseAsync(
            analyseService.AnalyseSharesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });

    /// <summary>
    /// Отчет по акции (по каждому типу анализа)
    /// </summary>        
    [HttpPost("report/total-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportShareTotalAnalyseAsync(
        [FromBody] GetReportAnalyseByTickerRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportShareTotalAnalyseAsync(request),
            result => new BaseResponse<ReportData>
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
    public Task<IActionResult> ReportSharesAnalyseSupertrendAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportSharesAnalyseSupertrendAsync(request),
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
    public Task<IActionResult> ReportSharesAnalyseCandleSequenceAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportSharesAnalyseCandleSequenceAsync(request),
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
    public Task<IActionResult> ReportSharesAnalyseCandleVolumeAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportSharesAnalyseCandleVolumeAsync(request),
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
    public Task<IActionResult> ReportSharesAnalyseRsiAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportSharesAnalyseRsiAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет по дивидендам
    /// </summary>        
    [HttpGet("report/dividends")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportDividendsAsync() =>
        GetResponseAsync(
            () => reportService.GetReportDividendsAsync(),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет по фундаментальным данным
    /// </summary>        
    [HttpGet("report/asset-fundamentals")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportAssetFundamentalsAsync() =>
        GetResponseAsync(
            () => reportService.GetReportAssetFundamentalsAsync(),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
}