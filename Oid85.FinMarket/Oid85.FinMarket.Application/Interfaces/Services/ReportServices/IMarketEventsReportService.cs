using Oid85.FinMarket.Application.Models.Reports;

namespace Oid85.FinMarket.Application.Interfaces.Services.ReportServices;

/// <summary>
/// Сервис отчетов по событиям
/// </summary>
public interface IMarketEventsReportService
{
    /// <summary>
    /// Отчет Активные рыночные события
    /// </summary>
    Task<ReportData> GetActiveMarketEventsAnalyseAsync();
}