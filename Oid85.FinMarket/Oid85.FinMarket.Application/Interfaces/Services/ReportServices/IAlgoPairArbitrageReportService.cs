using Oid85.FinMarket.Application.Models.BacktestResults;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Interfaces.Services.ReportServices;

public interface IAlgoPairArbitrageReportService
{
    Task<ReportData> GetStrategySignalsAsync();
    Task<ReportData> GetGroupByTickerStrategySignalsAsync();
    Task<ReportData> GetBacktestResultsAsync(TickerStrategyRequest request);
    Task<PairArbitrageBacktestResultData> GetBacktestResultByIdAsync(IdRequest request);
    Task<PairArbitrageBacktestResultData> GetBacktestResultByTickerAsync(TickerRequest request);
    Task<PairArbitrageBacktestResultData> GetBacktestResultPortfolioAsync();
}