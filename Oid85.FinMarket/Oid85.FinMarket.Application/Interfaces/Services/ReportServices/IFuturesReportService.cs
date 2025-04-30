using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Interfaces.Services.ReportServices;

/// <summary>
/// Сервис отчетов по фьючерсам
/// </summary>
public interface IFuturesReportService
{
    /// <summary>
    /// Отчет Аггрегированный анализ
    /// </summary>
    Task<ReportData> GetAggregatedAnalyseAsync(DateRangeRequest request);

    /// <summary>
    /// Отчет Анализ Супертренд
    /// </summary>
    Task<ReportData> GetSupertrendAnalyseAsync(DateRangeRequest request);

    /// <summary>
    /// Отчет Анализ Последовательность свечей одного цвета
    /// </summary>
    Task<ReportData> GetCandleSequenceAnalyseAsync(DateRangeRequest request);

    /// <summary>
    /// Отчет Анализ Растущий объем
    /// </summary>
    Task<ReportData> GetCandleVolumeAnalyseAsync(DateRangeRequest request);

    /// <summary>
    /// Отчет Анализ RSI
    /// </summary>
    Task<ReportData> GetRsiAnalyseAsync(DateRangeRequest request);
    
    /// <summary>
    /// Отчет Анализ ATR
    /// </summary>
    Task<ReportData> GetAtrAnalyseAsync(DateRangeRequest request);
    
    /// <summary>
    /// Отчет Анализ Donchian
    /// </summary>
    Task<ReportData> GetDonchianAnalyseAsync(DateRangeRequest request);    
    
    /// <summary>
    /// Отчет Доходность LTM
    /// </summary>
    Task<ReportData> GetYieldLtmAnalyseAsync(DateRangeRequest request);
    
    /// <summary>
    /// Отчет Анализ спреда
    /// </summary>
    Task<ReportData> GetSpreadAnalyseAsync(TickerListRequest request);
    
    /// <summary>
    /// Отчет Рыночные события
    /// </summary>
    Task<ReportData> GetActiveMarketEventsAnalyseAsync(TickerListRequest request);
}