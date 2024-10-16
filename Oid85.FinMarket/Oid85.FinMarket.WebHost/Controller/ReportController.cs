using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Application.Services;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.WebHost.Controller
{
    [Route("api")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReportService _reportService;        

        public ReportController(
            ILogger logger,
            IReportService reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }

        /// <summary>
        /// Отчет по анализу Супертренд
        /// </summary>        
        [HttpPost("report/analyse-supertrend/stocks")]
        [ProducesResponseType(typeof(GetReportAnalyseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetReportAnalyseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GetReportAnalyseResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReportAnalyseSupertrendStocksAsync(
            [FromBody] GetReportAnalyseRequest request)
        {
            _logger.Trace($"Request - /api/report/analyse-supertrend/stocks");

            try
            {
                var result = await _reportService.GetReportAnalyseSupertrendStocks(request);
                
                var response = new GetReportAnalyseResponse(result);
                
                return Ok(response);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                
                var error = new ResponseError()
                {
                    Code = 500,
                    Description = "Ошибка при выполнении запроса",
                    Message = exception.Message
                };

                var response = new GetReportAnalyseResponse(error);

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Отчет по анализу Последовательность свечей одного цвета
        /// </summary>        
        [HttpPost("report/analyse-candle-sequence/stocks")]
        [ProducesResponseType(typeof(GetReportAnalyseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetReportAnalyseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GetReportAnalyseResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReportAnalyseCandleSequenceStocksAsync(
            [FromBody] GetReportAnalyseRequest request)
        {
            _logger.Trace($"Request - /api/report/analyse-candle-sequence/stocks");

            try
            {
                var result = await _reportService.GetReportAnalyseCandleSequenceStocks(request);

                var response = new GetReportAnalyseResponse(result);

                return Ok(response);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);

                var error = new ResponseError()
                {
                    Code = 500,
                    Description = "Ошибка при выполнении запроса",
                    Message = exception.Message
                };

                var response = new GetReportAnalyseResponse(error);

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
