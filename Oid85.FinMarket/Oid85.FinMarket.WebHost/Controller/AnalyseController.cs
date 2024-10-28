using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Application.Services;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller
{
    [Route("api")]
    [ApiController]
    public class AnalyseController : FinMarketBaseController
    {
        private readonly IAnalyseService _analyseService;

        public AnalyseController(
            IAnalyseService analyseService)
        {
            _analyseService = analyseService;
        }

        /// <summary>
        /// Выполнить анализ
        /// </summary>
        [HttpGet("analyse-stocks")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> AnalyseStocksAsync() =>
            GetResponseAsync<object, BaseResponse<object>>(
                _analyseService.AnalyseStocksAsync);
    }
}
