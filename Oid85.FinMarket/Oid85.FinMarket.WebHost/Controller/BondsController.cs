using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/[controller]")]
[ApiController]
public class BondsController(
    ILoadService loadService,
    IAnalyseService analyseService,
    IBondsReportService reportService,
    IInstrumentService instrumentService) 
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
            async () => (await instrumentService.GetBondsInWatchlist())
                .Select(x => x.Ticker)
                .ToList(),
            result => new BaseResponse<List<string>>
            {
                Result = result
            });
    
    /// <summary>
    /// Получить тикеры отфильтрованных облигаций
    /// </summary>
    [HttpGet("filter-list-tickers")]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBondsFilterListAsync() =>
        await GetResponseAsync(
            async () => (await instrumentService.GetBondsByFilter())
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
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondsAsync() =>
        GetResponseAsync(
            loadService.LoadBondsAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
        
    /// <summary>
    /// Подгрузить свечи по облигаций
    /// </summary>
    [HttpGet("load-daily-candles")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondDailyCandlesAsync() =>
        GetResponseAsync(
            loadService.LoadBondDailyCandlesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Загрузить последние цены облигаций
    /// </summary>
    [HttpGet("load-last-prices")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondLastPricesAsync() =>
        GetResponseAsync(
            loadService.LoadBondLastPricesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Загрузить данные о купонах
    /// </summary>
    [HttpGet("load-bond-coupons")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadBondCouponsAsync() =>
        GetResponseAsync(
            loadService.LoadBondCouponsAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Выполнить анализ облигаций
    /// </summary>
    [HttpGet("daily-analyse")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> DailyAnalyseBondsAsync() =>
        GetResponseAsync(
            analyseService.DailyAnalyseBondsAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Сводный анализ
    /// </summary>
    [HttpPost("report/aggregated-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetAggregatedAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetAggregatedAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Супертренд
    /// </summary>
    [HttpPost("report/supertrend-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetSupertrendAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetSupertrendAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Последовательность свечей одного цвета
    /// </summary>
    [HttpPost("report/candle-sequence-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetCandleSequenceAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetCandleSequenceAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Растущий объем
    /// </summary>
    [HttpPost("report/candle-volume-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetCandleVolumeAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetCandleVolumeAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Купоны
    /// </summary>
    [HttpPost("report/coupon-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetCouponAnalyseAsync() =>
        GetResponseAsync(
            reportService.GetCouponAnalyseAsync,
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Подборка облигаций
    /// </summary>
    [HttpPost("report/bond-selection")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetBondSelectionAsync() =>
        GetResponseAsync(
            reportService.GetBondSelectionAsync,
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Рыночные события
    /// </summary>
    [HttpPost("report/active-market-events-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetActiveMarketEventsAnalyseAsync() =>
        GetResponseAsync(
            reportService.GetActiveMarketEventsAnalyseAsync,
            result => new BaseResponse<ReportData>
            {
                Result = result
            });    
}