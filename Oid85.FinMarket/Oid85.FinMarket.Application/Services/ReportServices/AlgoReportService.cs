using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Services.ReportServices;

public class AlgoReportService(
    IReportDataFactory reportDataFactory) 
    : IAlgoReportService
{
    public Task<ReportData> GetStrategySignalsAsync() =>
        reportDataFactory.CreateStrategySignalsReportDataAsync();
    
    public Task<ReportData> GetBacktestResultsAsync() =>
        reportDataFactory.CreateBacktestResultsReportDataAsync();

    public async Task<BacktestResult> GetBacktestResultByIdAsync(IdRequest request)
    {
        throw new NotImplementedException();
    }
}