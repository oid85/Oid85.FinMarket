using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Interfaces.Services.ReportServices;

/// <summary>
/// Сервис отчетов по индексам
/// </summary>
public interface IIndexesReportService
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
    /// Отчет Анализ RSI
    /// </summary>
    Task<ReportData> GetRsiAnalyseAsync(DateRangeRequest request);
    
    /// <summary>
    /// Отчет Доходность LTM
    /// </summary>
    Task<ReportData> GetYieldLtmAnalyseAsync(DateRangeRequest request);
    
    /// <summary>
    /// Отчет Максимальная просадка от максимума
    /// </summary>
    Task<ReportData> GetDrawdownFromMaximumAnalyseAsync(DateRangeRequest request);  
    
    /// <summary>
    /// Отчет Рыночные события
    /// </summary>
    Task<ReportData> GetActiveMarketEventsAnalyseAsync(TickerListRequest request);
}