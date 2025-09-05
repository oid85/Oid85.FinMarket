using Oid85.FinMarket.Application.Models.Reports;

namespace Oid85.FinMarket.Application.Interfaces.Factories;

public interface IReportDataFactory
{
    Task<ReportData> CreateReportDataAsync(List<Guid> instrumentIds, string analyseType, DateOnly from, DateOnly to);
    Task<ReportData> CreateAggregatedReportDataAsync(List<Guid> instrumentIds, List<string> analyseTypes, DateOnly from, DateOnly to);
    Task<ReportData> CreateDividendInfoReportDataAsync(List<Guid> instrumentIds);
    Task<ReportData> CreateShareMultiplicatorReportDataAsync(List<Guid> instrumentIds);
    Task<ReportData> CreateBankMultiplicatorReportDataAsync(List<Guid> getInstrumentIds);
    Task<ReportData> CreateForecastTargetReportDataAsync(List<Guid> instrumentIds);
    Task<ReportData> CreateForecastConsensusReportDataAsync(List<Guid> instrumentIds);
    Task<ReportData> CreateBondCouponReportDataAsync(List<Guid> instrumentIds);
    Task<ReportData> CreateActiveMarketEventsReportDataAsync(List<Guid> instrumentIds);
    Task<ReportData> CreateAssetReportEventsReportDataAsync(List<Guid> instrumentIds);
    Task<ReportData> CreateFearGreedIndexReportDataAsync(DateOnly from, DateOnly to);
    Task<ReportData> CreateStrategySignalsReportDataAsync();
    Task<ReportData> CreateBacktestResultsReportDataAsync(string ticker, string strategyName);
    Task<ReportData> CreateBacktestResultReportDataAsync(Guid backtestResultId);
    Task<ReportData> CreatePairArbitrageStrategySignalsReportDataAsync();
    Task<ReportData> CreateGroupByTickerPairArbitrageStrategySignalsReportDataAsync();
    Task<ReportData> CreatePairArbitrageBacktestResultsReportDataAsync(string ticker, string strategyName);
    Task<ReportData> CreatePairArbitrageBacktestResultReportDataAsync(Guid backtestResultId);
}