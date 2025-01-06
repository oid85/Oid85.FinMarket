using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
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
    ICurrenciesReportService reportService,
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
    /// Отчет Сводный анализ
    /// </summary>
    [HttpGet("report/aggregate-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetAggregatedAnalyseAsync(
        [FromBody] GetAnalyseByTickerRequest request) =>
        GetResponseAsync(
            () => reportService.GetAggregatedAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Супертренд
    /// </summary>
    [HttpGet("report/supertrend-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetSupertrendAnalyseAsync(
        [FromBody] GetAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetSupertrendAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Последовательность свечей одного цвета
    /// </summary>
    [HttpGet("report/candle-sequence-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetCandleSequenceAnalyseAsync(
        [FromBody] GetAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetCandleSequenceAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет RSI
    /// </summary>
    [HttpGet("report/rsi-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetRsiAnalyseAsync(
        [FromBody] GetAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetRsiAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Доходность LTM
    /// </summary>
    [HttpGet("report/yield-ltm-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetYieldLtmAnalyseAsync(
        [FromBody] GetAnalyseRequest request) =>
        GetResponseAsync(
            () => reportService.GetYieldLtmAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
}