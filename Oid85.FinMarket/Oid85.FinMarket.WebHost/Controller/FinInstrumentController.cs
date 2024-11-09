using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller
{
    [Route("api")]
    [ApiController]
    public class FinInstrumentController : FinMarketBaseController
    {
        private readonly IShareRepository _shareRepository;        

        public FinInstrumentController(
            IShareRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        /// <summary>
        /// Получить инструменты из листа наблюдения
        /// </summary>
        [HttpGet("fin-instrument/watch-list")]
        [ProducesResponseType(typeof(BaseResponse<List<Share>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<List<Share>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<List<Share>>), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> GetWatchListAsync() =>
            GetResponseAsync(
                _shareRepository.GetWatchListSharesAsync,
                result => new BaseResponse<List<Share>>
                {
                    Result = result
                });        
    }
}
