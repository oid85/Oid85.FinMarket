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
public class CurrenciesController(
    ILoadService loadService,
    IAnalyseService analyseService,
    IReportService reportService,
    ICurrencyRepository currencyRepository) 
    : FinMarketBaseController
{
    /// <summary>
    /// Получить тикеры валют из листа наблюдения
    /// </summary>
    [HttpGet("watch-list-tickers")]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCurrenciesWatchListAsync() =>
        await GetResponseAsync(
            async () => (await currencyRepository.GetWatchListAsync())
                .Select(x => x.Ticker)
                .ToList(),
            result => new BaseResponse<List<string>>
            {
                Result = result
            });
    
    /// <summary>
    /// Загрузить справочник валют
    /// </summary>
    [HttpGet("load-catalog")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadCurrenciesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadCurrenciesAsync);
        
    /// <summary>
    /// Подгрузить свечи по валют
    /// </summary>
    [HttpGet("load-daily-candles")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadCurrencyDailyCandlesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadCurrencyDailyCandlesAsync);
    
    /// <summary>
    /// Загрузить последние цены валют
    /// </summary>
    [HttpGet("load-last-prices")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadCurrencyLastPricesAsync() =>
        GetResponseAsync<object, BaseResponse<object>>(
            loadService.LoadCurrencyLastPricesAsync);
    
    /// <summary>
    /// Выполнить анализ валют
    /// </summary>
    [HttpGet("analyse")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> AnalyseCurrenciesAsync() =>
        GetResponseAsync(
            analyseService.AnalyseCurrenciesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет по анализу Супертренд
    /// </summary>        
    [HttpPost("report/analyse-supertrend")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportCurrenciesAnalyseSupertrendAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportCurrenciesAnalyseSupertrendAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет по анализу Последовательность свечей одного цвета
    /// </summary>        
    [HttpPost("report/analyse-candle-sequence")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> ReportCurrenciesAnalyseCandleSequenceAsync(
        [FromBody] GetReportAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetReportCurrenciesAnalyseCandleSequenceAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
}