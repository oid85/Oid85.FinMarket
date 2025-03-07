using Oid85.FinMarket.Application.Models.Reports;

namespace Oid85.FinMarket.Application.Interfaces.Factories;

public interface IReportDataFactory
{
    Task<ReportData> CreateReportDataAsync(List<Guid> instrumentIds, string analyseType, DateOnly from, DateOnly to);
    Task<ReportData> CreateAggregatedReportDataAsync(List<Guid> instrumentIds, List<string> analyseTypes, DateOnly from, DateOnly to);
    Task<ReportData> CreateDividendInfoReportDataAsync();
    Task<ReportData> CreateMultiplicatorReportDataAsync();
    Task<ReportData> CreateForecastTargetReportDataAsync();
    Task<ReportData> CreateForecastConsensusReportDataAsync();
    Task<ReportData> CreateBondCouponReportDataAsync(List<Guid> instrumentIds);
    Task<ReportData> CreateSpreadReportDataAsync();
    Task<ReportData> CreateActiveMarketEventsReportDataAsync(List<Guid> instrumentIds);
}