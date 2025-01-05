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
}