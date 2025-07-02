using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Services.Algo;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.BacktestResults;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Services.ReportServices;

public class AlgoReportService(
    IReportDataFactory reportDataFactory,
    IDiagramDataFactory diagramDataFactory,
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

        var backtestResultData = new BacktestResultData
        {
            Report = await reportDataFactory.CreateBacktestResultReportDataAsync(request.Id),
            Diagram = await diagramDataFactory.CreateBacktestResultDiagramDataAsync(result.strategy!)
        };

        return backtestResultData;
    }
}