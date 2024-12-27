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
    [HttpPost("report/analyse/stocks")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportAnalyseStockAsync(
        [FromBody] GetReportAnalyseStockRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportAnalyseStock(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });

    /// <summary>
    /// Отчет по анализу Супертренд
    /// </summary>        
    [HttpPost("report/analyse-supertrend/stocks")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportAnalyseSupertrendStocksAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportAnalyseSupertrendStocks(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });

    /// <summary>
    /// Отчет по анализу Последовательность свечей одного цвета
    /// </summary>        
    [HttpPost("report/analyse-candle-sequence/stocks")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportAnalyseCandleSequenceStocksAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportAnalyseCandleSequenceStocks(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });

    /// <summary>
    /// Отчет по анализу Растущий объем
    /// </summary>        
    [HttpPost("report/analyse-candle-volume/stocks")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportAnalyseCandleVolumeStocksAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportAnalyseCandleVolumeStocks(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });

    /// <summary>
    /// Отчет по анализу Rsi
    /// </summary>        
    [HttpPost("report/analyse-rsi/stocks")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportAnalyseRsiStocksAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportAnalyseRsiStocks(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });

    /// <summary>
    /// Отчет по доходности LTM
    /// </summary>        
    [HttpPost("report/analyse-yield-ltm/indexes")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportAnalyseYieldLtmIndexesAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.ReportAnalyseYieldLtmIndexes(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет по дивидендам
    /// </summary>        
    [HttpGet("report/dividends/stocks")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportDividendsStocksAsync() =>
        GetResponseAsync(
            () => reportService.GetReportDividendsStocks(),
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
            () => reportService.GetReportBonds(),
            result => new BaseResponse<ReportData>
            {
                Result = result
            }); 
    
    /// <summary>
    /// Отчет по фундаментальным данным
    /// </summary>        
    [HttpGet("report/asset-fundamentals/stocks")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportAssetFundamentalsStocksAsync() =>
        GetResponseAsync(
            () => reportService.GetReportAssetFundamentalsStocks(),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
}