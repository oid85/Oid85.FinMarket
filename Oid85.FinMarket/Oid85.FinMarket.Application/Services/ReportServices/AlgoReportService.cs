using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.BacktestResults;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services.ReportServices;

public class AlgoReportService(
    IReportDataFactory reportDataFactory,
    IDiagramDataFactory diagramDataFactory,
    IAlgoService algoService,
    IBacktestResultRepository backtestResultRepository,
    IResourceStoreService resourceStoreService) 
    : IAlgoReportService
{
    public Task<ReportData> GetStrategySignalsAsync() =>
        reportDataFactory.CreateStrategySignalsReportDataAsync();
    
    public Task<ReportData> GetBacktestResultsAsync(TickerStrategyRequest request) =>
        reportDataFactory.CreateBacktestResultsReportDataAsync(request.Ticker, request.StrategyName);

    public async Task<BacktestResultData> GetBacktestResultByIdAsync(IdRequest request)
    {
        var result = await algoService.BacktestAsync(request.Id);

        var backtestResultData = new BacktestResultData
        {
            ReportData = await reportDataFactory.CreateBacktestResultReportDataAsync(request.Id),
            DiagramData = await diagramDataFactory.CreateBacktestResultDiagramDataAsync(result.strategy!)
        };

        backtestResultData.DiagramData.Title = $"{result.backtestResult!.Ticker} {result.backtestResult!.StrategyName}";
        
        return backtestResultData;
    }

    public async Task<BacktestResultData> GetBacktestResultByTickerAsync(TickerRequest request)
    {
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var backtestResults = await backtestResultRepository.GetAsync(algoConfigResource.BacktestResultFilterResource);

        var strategies = new List<Strategy>();
        
        foreach (var backtestResult in backtestResults.Where(x => x.Ticker == request.Ticker))
        {
            var result = await algoService.BacktestAsync(backtestResult.Id);
            strategies.Add(result.strategy!);
        }

        var backtestResultData = new BacktestResultData
        {
            DiagramData = await diagramDataFactory.CreateBacktestResultDiagramDataAsync(strategies)
        };

        backtestResultData.DiagramData.Title = $"{request.Ticker}";
        
        return backtestResultData;
    }

    public async Task<BacktestResultData> GetBacktestResultPortfolioAsync()
    {
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var backtestResults = await backtestResultRepository.GetAsync(algoConfigResource.BacktestResultFilterResource);

        var strategies = new List<Strategy>();
        
        foreach (var backtestResult in backtestResults)
        {
            var result = await algoService.BacktestAsync(backtestResult.Id);
            strategies.Add(result.strategy!);
        }

        var backtestResultData = new BacktestResultData
        {
            DiagramData = await diagramDataFactory.CreateBacktestResultWithoutPriceDiagramDataAsync(strategies)
        };

        backtestResultData.DiagramData.Title = "Портфель";
        
        return backtestResultData;
    }
}