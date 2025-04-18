﻿using Microsoft.AspNetCore.Mvc;
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

[Route("api/[controller]")]
[ApiController]
public class SharesController(
    ILoadService loadService,
    IAnalyseService analyseService,
    ISharesReportService reportService,
    ISharesDiagramService diagramService,
    ITickerListUtilService tickerListUtilService)
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
            async () => (await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.SharesWatchlist))
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
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadSharesAsync() =>
        GetResponseAsync(
            loadService.LoadSharesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });

    /// <summary>
    /// Подгрузить дневные свечи по акциям
    /// </summary>
    [HttpGet("load-daily-candles")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadShareDailyCandlesAsync() =>
        GetResponseAsync(
            loadService.LoadShareDailyCandlesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });

    /// <summary>
    /// Загрузить 5-минутные свечи по акциям
    /// </summary>
    [HttpGet("load-five-minute-candles")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadShareFiveMinuteCandlesAsync() =>
        GetResponseAsync(
            loadService.LoadShareFiveMinuteCandlesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Загрузить прогнозы
    /// </summary>
    [HttpGet("load-forecasts")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadForecastsAsync() =>
        GetResponseAsync(
            loadService.LoadForecastsAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Загрузить данные о дивидендах
    /// </summary>
    [HttpGet("load-dividends")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadDividendInfosAsync() =>
        GetResponseAsync(
            loadService.LoadDividendInfosAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Загрузить последние цены акций
    /// </summary>
    [HttpGet("load-last-prices")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadShareLastPricesAsync() =>
        GetResponseAsync(
            loadService.LoadShareLastPricesAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });

    /// <summary>
    /// Загрузить отчеты по эмитентам
    /// </summary>
    [HttpGet("load-asset-report-events")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> LoadAssetReportEventsAsync() =>
        GetResponseAsync(
            loadService.LoadAssetReportEventsAsync,
            result => new BaseResponse<bool>
            {
                Result = result
            });
    
    /// <summary>
    /// Выполнить анализ акций
    /// </summary>
    [HttpGet("daily-analyse")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> DailyAnalyseSharesAsync() =>
        GetResponseAsync(
            analyseService.DailyAnalyseSharesAsync,
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
    /// Диаграмма График цен закрытия (5 мин)
    /// </summary>
    [HttpPost("diagram/five-minutes-close-prices")]
    [ProducesResponseType(typeof(BaseResponse<SimpleDiagramData>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<SimpleDiagramData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<SimpleDiagramData>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetFiveMinutesClosePricesAsync(
        [FromBody] DateTimeRangeRequest request) =>
        GetResponseAsync(
            () => diagramService.GetFiveMinutesClosePricesAsync(request),
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