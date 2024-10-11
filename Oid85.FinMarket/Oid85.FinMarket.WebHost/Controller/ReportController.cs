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
        /// Отчет по анализу Супертренд для акций
        /// </summary>        
        [HttpPost("report/analyse-supertrend/stocks")]
        [ProducesResponseType(typeof(GetReportAnalyseSupertrendResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetReportAnalyseSupertrendResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GetReportAnalyseSupertrendResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReportAnalyseSupertrendStocksAsync(
            [FromBody] GetReportAnalyseSupertrendRequest request)
        {
            _logger.Trace($"Request - /api/report/analyse-supertrend/stocks");

            try
            {
                var result = await _reportService.GetReportAnalyseSupertrendStocks(request);
                
                var response = new GetReportAnalyseSupertrendResponse(result);
                
                return Ok(response);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                
                var error = new ResponseError()
                {
                    ErrorCode = 500,
                    ErrorDescription = "Ошибка при выполнении запроса",
                    ErrorMessage = exception.Message
                };

                var response = new GetReportAnalyseSupertrendResponse(error);

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
