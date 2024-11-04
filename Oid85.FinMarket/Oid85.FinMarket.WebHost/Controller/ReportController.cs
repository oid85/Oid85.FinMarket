using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Application.Models.Results;
using Oid85.FinMarket.Application.Services;
using Oid85.FinMarket.WebHost.Controller.Base;

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
                () => _reportService.GetReportAnalyseCandleVolumeStocks(request),
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
                () => _reportService.GetReportAnalyseRsiStocks(request),
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
                () => _reportService.GetReportDividendsStocks(),
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
                () => _reportService.GetReportBonds(),
                result => new BaseResponse<ReportData>
                {
                    Result = result
                });        
    }
}
