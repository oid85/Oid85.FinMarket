using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/[controller]")]
[ApiController]
public class BondsController(
    ILoadService loadService,
    IAnalyseService analyseService,
    IReportService reportService,
    IBondRepository bondRepository) 
    : FinMarketBaseController
{
    /// <summary>
    /// Получить тикеры облигаций из листа наблюдения
    /// </summary>
    [HttpGet("watch-list-tickers")]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBondsWatchListAsync() =>
        await GetResponseAsync(
            async () => (await bondRepository.GetWatchListAsync())
                .Select(x => x.Ticker)
                .ToList(),
            result => new BaseResponse<List<string>>
            {
                Result = result
            });
    
    /// <summary>
    /// Загрузить справочник облигаций
    /// </summary>
    [HttpGet("load-catalog")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadBondsAsync);
        
    /// <summary>
    /// Подгрузить свечи по облигаций
    /// </summary>
    [HttpGet("load-daily-candles")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondDailyCandlesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadBondDailyCandlesAsync);
    
    /// <summary>
    /// Загрузить последние цены облигаций
    /// </summary>
    [HttpGet("load-last-prices")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondLastPricesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadBondLastPricesAsync);
    
    /// <summary>
    /// Загрузить данные о купонах
    /// </summary>
    [HttpGet("load-bond-coupons")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondCouponsAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadBondCouponsAsync);
    
    /// <summary>
    /// Выполнить анализ облигаций
    /// </summary>
    [HttpGet("analyse")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> AnalyseBondsAsync() =>
        GetResponseAsync(
            analyseService.AnalyseBondsAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
}