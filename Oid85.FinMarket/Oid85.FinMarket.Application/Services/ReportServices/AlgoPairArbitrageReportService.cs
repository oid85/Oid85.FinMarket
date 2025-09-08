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

public class AlgoPairArbitrageReportService(
    IReportDataFactory reportDataFactory,
    IDiagramDataFactory diagramDataFactory,
    IPairArbitrageService service,
    IPairArbitrageBacktestResultRepository backtestResultRepository,
    IResourceStoreService resourceStoreService) 
    : IAlgoPairArbitrageReportService
{
    public Task<ReportData> GetStrategySignalsAsync() =>
        reportDataFactory.CreatePairArbitrageStrategySignalsReportDataAsync();

    public Task<ReportData> GetGroupByTickerStrategySignalsAsync() =>
        reportDataFactory.CreateGroupByTickerPairArbitrageStrategySignalsReportDataAsync();

    public Task<ReportData> GetBacktestResultsAsync(TickerStrategyRequest request) =>
        reportDataFactory.CreatePairArbitrageBacktestResultsReportDataAsync(request.Ticker, request.StrategyName);

    public async Task<PairArbitrageBacktestResultData> GetBacktestResultByIdAsync(IdRequest request)
    {
        var result = await service.BacktestAsync(request.Id);

        var backtestResultData = new PairArbitrageBacktestResultData
        {
            ReportData = await reportDataFactory.CreatePairArbitrageBacktestResultReportDataAsync(request.Id),
            DiagramData = await diagramDataFactory.CreatePairArbitrageBacktestResultDiagramDataAsync(result.strategy!)
        };

        backtestResultData.DiagramData.Title = $"{result.backtestResult!.TickerFirst} vs. {result.backtestResult!.TickerFirst} {result.backtestResult!.StrategyName}";
        
        return backtestResultData;
    }

    public async Task<PairArbitrageBacktestResultData> GetBacktestResultByTickerAsync(TickerRequest request)
    {
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var backtestResults = await backtestResultRepository.GetAsync(algoConfigResource.PairArbitrageBacktestResultFilterResource);

        var strategies = new List<PairArbitrageStrategy>();
        
        foreach (var backtestResult in backtestResults.Where(x => request.Ticker == $"{x.TickerFirst},{x.TickerSecond}"))
        {
            var result = await service.BacktestAsync(backtestResult.Id);
            strategies.Add(result.strategy!);
        }

        var backtestResultData = new PairArbitrageBacktestResultData
        {
            DiagramData = await diagramDataFactory.CreatePairArbitrageBacktestResultDiagramDataAsync(strategies)
        };

        backtestResultData.DiagramData.Title = $"{request.Ticker}";
        
        return backtestResultData;
    }

    public async Task<PairArbitrageBacktestResultData> GetBacktestResultPortfolioAsync()
    {
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var backtestResults = await backtestResultRepository.GetAsync(algoConfigResource.PairArbitrageBacktestResultFilterResource);

        var strategies = new List<PairArbitrageStrategy>();
        
        foreach (var backtestResult in backtestResults)
        {
            var result = await service.BacktestAsync(backtestResult.Id);
            strategies.Add(result.strategy!);
        }

        var backtestResultData = new PairArbitrageBacktestResultData
        {
            DiagramData = await diagramDataFactory.CreatePairArbitrageBacktestResultWithoutPriceDiagramDataAsync(strategies)
        };

        backtestResultData.DiagramData.Title = "Портфель";
        
        return backtestResultData;
    }
}