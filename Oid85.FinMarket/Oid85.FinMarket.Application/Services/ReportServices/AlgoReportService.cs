using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Services.Algo;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.BacktestResults;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Services.ReportServices;

public class AlgoReportService(
    IReportDataFactory reportDataFactory,
    IBacktestService backtestService) 
    : IAlgoReportService
{
    public Task<ReportData> GetStrategySignalsAsync() =>
        reportDataFactory.CreateStrategySignalsReportDataAsync();
    
    public Task<ReportData> GetBacktestResultsAsync() =>
        reportDataFactory.CreateBacktestResultsReportDataAsync();

    public async Task<BacktestResultData> GetBacktestResultByIdAsync(IdRequest request)
    {
        var result = await backtestService.BacktestAsync(request.Id);

        var backtestResultData = new BacktestResultData();
        return backtestResultData;
    }
}