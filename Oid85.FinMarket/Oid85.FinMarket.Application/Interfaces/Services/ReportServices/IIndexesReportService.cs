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
    Task<ReportData> GetAggregatedAnalyseAsync(
        GetAnalyseRequest request);

    /// <summary>
    /// Отчет Анализ Супертренд
    /// </summary>
    Task<ReportData> GetSupertrendAnalyseAsync(
        GetAnalyseRequest request);

    /// <summary>
    /// Отчет Анализ Последовательность свечей одного цвета
    /// </summary>
    Task<ReportData> GetCandleSequenceAnalyseAsync(
        GetAnalyseRequest request);

    /// <summary>
    /// Отчет Анализ RSI
    /// </summary>
    Task<ReportData> GetRsiAnalyseAsync(
        GetAnalyseRequest request);
    
    /// <summary>
    /// Отчет Доходность LTM
    /// </summary>
    Task<ReportData> GetYieldLtmAnalyseAsync(
        GetAnalyseRequest request);
}