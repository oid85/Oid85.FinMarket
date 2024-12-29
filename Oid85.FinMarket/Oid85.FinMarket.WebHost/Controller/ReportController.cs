using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api")]
[ApiController]
public class ReportController(IReportService reportService) : FinMarketBaseController
{
    /// <summary>
    /// Отчет по акции
    /// </summary>        
    [HttpPost("report/stocks/analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportStocksAnalyseAsync(
        [FromBody] GetReportAnalyseStockRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportStockAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });

    /// <summary>
    /// Отчет по анализу Супертренд
    /// </summary>        
    [HttpPost("report/stocks/analyse-supertrend")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportStocksAnalyseSupertrendAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportStocksAnalyseSupertrendAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });

    /// <summary>
    /// Отчет по анализу Последовательность свечей одного цвета
    /// </summary>        
    [HttpPost("report/stocks/analyse-candle-sequence")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportStocksAnalyseCandleSequenceAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportStocksAnalyseCandleSequenceAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });

    /// <summary>
    /// Отчет по анализу Растущий объем
    /// </summary>        
    [HttpPost("report/stocks/analyse-candle-volume")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportStocksAnalyseCandleVolumeAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportStocksAnalyseCandleVolumeAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });

    /// <summary>
    /// Отчет по анализу Rsi
    /// </summary>        
    [HttpPost("report/stocks/analyse-rsi")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportStocksAnalyseRsiAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportStocksAnalyseRsiAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });

    /// <summary>
    /// Отчет по доходности LTM
    /// </summary>        
    [HttpPost("report/indexes/analyse-yield-ltm")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportIndexesAnalyseYieldLtmAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.ReportIndexesAnalyseYieldLtmAsync(request),
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
    
    /// <summary>
    /// Отчет по спредам
    /// </summary>        
    [HttpGet("report/spreads")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportSpreadsAsync() =>
        GetResponseAsync(
            () => reportService.ReportSpreadsAsync(),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
}