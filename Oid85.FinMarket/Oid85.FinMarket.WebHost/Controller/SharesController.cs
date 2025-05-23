using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.DiagramServices;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Responses;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.WebHost.Controller.Base;

namespace Oid85.FinMarket.WebHost.Controller;

[Route("api/shares")]
[ApiController]
public class SharesController(
    ISharesReportService reportService,
    ISharesDiagramService diagramService,
    ITickerListUtilService tickerListUtilService,
    ILoadService loadService)
    : FinMarketBaseController
{
    /// <summary>
    /// Подгрузить историю дневных свечей
    /// </summary>
    [HttpGet("load-history-daily-candles")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadHistoryShareDailyCandlesAsync() =>
        GetResponseAsync(
            loadService.LoadHistoryShareDailyCandlesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Подгрузить историю часовых свечей
    /// </summary>
    [HttpGet("load-history-hourly-candles")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadHistoryShareHourlyCandlesAsync() =>
        GetResponseAsync(
            loadService.LoadHistoryShareHourlyCandlesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });    
    
    /// <summary>
    /// Получить тикеры акций из листа наблюдения
    /// </summary>
    [HttpGet("watch-list-tickers")]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSharesWatchListAsync() =>
        await GetResponseAsync(
            async () => (await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.SharesWatchlist))
                .Select(x => x.Ticker)
                .ToList(),
            result => new BaseResponse<List<string>>
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
    /// Отчет RSI
    /// </summary>
    [HttpPost("report/rsi-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetRsiAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetRsiAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет ATR
    /// </summary>
    [HttpPost("report/atr-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetAtrAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetAtrAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Donchian
    /// </summary>
    [HttpPost("report/donchian-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetDonchianAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetDonchianAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });    
    
    /// <summary>
    /// Отчет Доходность LTM
    /// </summary>
    [HttpPost("report/yield-ltm-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetYieldLtmAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetYieldLtmAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Максимальная просадка от максимума
    /// </summary>
    [HttpPost("report/drawdown-from-maximum-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetDrawdownFromMaximumAnalyseAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetDrawdownFromMaximumAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });    
    
    /// <summary>
    /// Отчет Дивиденды
    /// </summary>
    [HttpPost("report/dividend-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetDividendAnalyseAsync(
        [FromBody] TickerListRequest request) =>
        GetResponseAsync(
            () => reportService.GetDividendAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Мультипликаторы
    /// </summary>
    [HttpPost("report/multiplicator-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetMultiplicatorAnalyseAsync(
        [FromBody] TickerListRequest request) =>
        GetResponseAsync(
            () => reportService.GetMultiplicatorAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Прогнозы
    /// </summary>
    [HttpPost("report/forecast-target-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetForecastTargetAnalyseAsync(
        [FromBody] TickerListRequest request) =>
        GetResponseAsync(
            () => reportService.GetForecastTargetAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            }); 
    
    /// <summary>
    /// Отчет Консенсус-прогнозы
    /// </summary>
    [HttpPost("report/forecast-consensus-analyse")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetForecastConsensusAnalyseAsync(
        [FromBody] TickerListRequest request) =>
        GetResponseAsync(
            () => reportService.GetForecastConsensusAnalyseAsync(request),
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
    public Task<IActionResult> GetActiveMarketEventsAnalyseAsync(
        [FromBody] TickerListRequest request) =>
        GetResponseAsync(
            () => reportService.GetActiveMarketEventsAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Отчеты по эмитентам
    /// </summary>
    [HttpPost("report/asset-report-events")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetAssetReportEventsAnalyseAsync(
        [FromBody] TickerListRequest request) =>
        GetResponseAsync(
            () => reportService.GetAssetReportEventsAnalyseAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Отчет Индекс страха и жадности
    /// </summary>
    [HttpPost("report/fear-greed-index")]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ReportData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetFearGreedIndexAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => reportService.GetFearGreedIndexAsync(request),
            result => new BaseResponse<ReportData>
            {
                Result = result
            });
    
    /// <summary>
    /// Диаграмма График цен закрытия (дневные)
    /// </summary>
    [HttpPost("diagram/daily-close-prices")]
    [ProducesResponseType(typeof(BaseResponse<SimpleDiagramData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<SimpleDiagramData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<SimpleDiagramData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetDailyClosePricesAsync(
        [FromBody] DateRangeRequest request) =>
        GetResponseAsync(
            () => diagramService.GetDailyClosePricesAsync(request),
            result => new BaseResponse<SimpleDiagramData>
            {
                Result = result
            });
    
    /// <summary>
    /// Диаграмма Мультипликаторы MCap, P/E, NetDebt/EBITDA
    /// </summary>
    [HttpPost("diagram/multiplicators-mcap-pe-netdebtebitda")]
    [ProducesResponseType(typeof(BaseResponse<BubbleDiagramData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<BubbleDiagramData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<BubbleDiagramData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetMultiplicatorsMCapPeNetDebtEbitdaAsync(
        [FromBody] TickerListRequest request) =>
        GetResponseAsync(
            () => diagramService.GetMultiplicatorsMCapPeNetDebtEbitdaAsync(request),
            result => new BaseResponse<BubbleDiagramData>
            {
                Result = result
            });     
}