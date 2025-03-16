using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Interfaces.Services.ReportServices;

/// <summary>
/// Сервис отчетов по облигациям
/// </summary>
public interface IBondsReportService
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
    /// Отчет Купоны
    /// </summary>
    Task<ReportData> GetCouponAnalyseAsync();

    /// <summary>
    /// Отчет Подборка облигаций
    /// </summary>
    Task<ReportData> GetBondSelectionAsync();

    /// <summary>
    /// Отчет Рыночные события
    /// </summary>
    Task<ReportData> GetActiveMarketEventsAnalyseAsync();
}