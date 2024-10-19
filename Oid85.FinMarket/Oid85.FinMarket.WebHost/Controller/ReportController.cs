using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Application.Models.Results;
using Oid85.FinMarket.Application.Services;
using Oid85.FinMarket.WebHost.Controller.Base;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.WebHost.Controller
{
    [Route("api")]
    [ApiController]
    public class ReportController : FinMarketBaseController
    {
        private readonly IReportService _reportService;        

        public ReportController(
            IReportService reportService)
        {
            _reportService = reportService;
        }

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
                () => _reportService.GetReportAnalyseStock(request),
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
                () => _reportService.GetReportAnalyseSupertrendStocks(request),
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
                () => _reportService.GetReportAnalyseCandleSequenceStocks(request),
                result => new BaseResponse<ReportData>
                {
                    Result = result
                });
    }
}
