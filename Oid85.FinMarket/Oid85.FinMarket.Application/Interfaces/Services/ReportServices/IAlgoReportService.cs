using Oid85.FinMarket.Application.Models.BacktestResults;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Interfaces.Services.ReportServices;

public interface IAlgoReportService
{
    Task<ReportData> GetStrategySignalsAsync();
    Task<ReportData> GetBacktestResultsAsync();
    Task<BacktestResultData> GetBacktestResultByIdAsync(IdRequest request);
}