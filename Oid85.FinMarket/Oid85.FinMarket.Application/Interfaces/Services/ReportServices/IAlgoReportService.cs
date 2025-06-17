using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Services.ReportServices;

public interface IAlgoReportService
{
    Task<ReportData> GetStrategySignalsAsync();
    Task<ReportData> GetBacktestResultsAsync();
    Task<BacktestResult> GetBacktestResultByIdAsync(IdRequest request);
}