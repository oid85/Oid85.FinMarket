using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services.Algo;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services.Algo;

public class BacktestService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IHourlyCandleRepository hourlyCandleRepository,
    IResourceStoreService resourceStoreService,
    IOptimizationResultRepository optimizationResultRepository,
    IBacktestResultRepository backtestResultRepository,
    IStrategySignalRepository strategySignalRepository,
    IServiceProvider serviceProvider) 
    : AlgoService(
        logger,
        dailyCandleRepository, 
        hourlyCandleRepository,
        resourceStoreService,
        serviceProvider), 
        IBacktestService
{
    private readonly IResourceStoreService _resourceStoreService = resourceStoreService;
    private readonly ILogger _logger = logger;

    public async Task<bool> BacktestAsync()
    {
        await InitBacktestAsync();
        
        var algoConfigResource = await _resourceStoreService.GetAlgoConfigAsync();
        var algoStrategyResources = await _resourceStoreService.GetAlgoStrategiesAsync();

        var optimizationResults = await optimizationResultRepository.GetAsync(algoConfigResource.OptimizationResultFilterResource);
        
        await backtestResultRepository.InvertDeleteAsync(algoStrategyResources.Select(x => x.Id).ToList());
        
        foreach (var (strategyId, strategy) in StrategyDictionary)
        {
            await backtestResultRepository.DeleteAsync(strategyId);
            
            var algoStrategyResource = algoStrategyResources.Find(x => x.Id == strategyId);
                
            if (algoStrategyResource is null)
                continue;
                
            if (!algoStrategyResource.Enable)
                continue;
            
            var backtestResults = new List<BacktestResult>();
            
            foreach (var optimizationResult in optimizationResults.Where(x => x.StrategyId == strategyId))
            {
                strategy.StabilizationPeriod = algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1;
                strategy.StartMoney = algoConfigResource.MoneyManagementResource.Money;
                strategy.EndMoney = algoConfigResource.MoneyManagementResource.Money;
                strategy.PercentOfMoney = algoConfigResource.MoneyManagementResource.PercentOfMoney;
                strategy.Ticker = optimizationResult.Ticker;
                
                strategy.Candles = algoStrategyResource.Timeframe switch
                {
                    "D" => DailyCandles.TryGetValue(strategy.Ticker, out var candles) ? candles : [],
                    "H" => HourlyCandles.TryGetValue(strategy.Ticker, out var candles) ? candles : [],
                    _ => []
                };

                if (strategy.Candles is [])
                    continue;
                
                if (strategy.Candles.Count < strategy.StabilizationPeriod + 1)
                    continue;
                
                var parameterSet = JsonSerializer.Deserialize<Dictionary<string, int>>(optimizationResult.StrategyParams);
                
                if (parameterSet is null)
                    continue;
                
                strategy.Parameters = parameterSet;
                strategy.StopLimits.Clear();
                strategy.Positions.Clear();
                strategy.EqiutyCurve.Clear();
                strategy.DrawdownCurve.Clear();
                strategy.EndMoney = algoConfigResource.MoneyManagementResource.Money;
                
                var sw = Stopwatch.StartNew();
                    
                try
                {
                    strategy.Execute();
                    
                    var backtestResult = CreateBacktestResult(strategy);
                    backtestResults.Add(backtestResult);
                }
                    
                catch (Exception exception)
                {
                    _logger.Error($"Ошибка '{algoStrategyResource.Name}', '{strategyId}', '{strategy.Ticker}', '{exception.Message}'");
                }
                    
                sw.Stop();
                    
                Debug.Print($"Бэктест '{algoStrategyResource.Name}', '{strategyId}', '{strategy.Ticker}', '{JsonSerializer.Serialize(parameterSet)}' {sw.Elapsed.TotalMilliseconds:N2} ms");
            }
            
            await backtestResultRepository.AddAsync(backtestResults);
        }
        
        await CalculateStrategySignalsAsync();
        
        return true;
    }

    public async Task<(BacktestResult? backtestResult, Strategy? strategy)> BacktestAsync(Guid backtestResultId)
    {
        var backtestResult = await backtestResultRepository.GetAsync(backtestResultId);
        
        if (backtestResult is null)
            return (null, null);
        
        await InitBacktestAsync(backtestResult.Ticker, backtestResult.StrategyId);
        
        var algoConfigResource = await _resourceStoreService.GetAlgoConfigAsync();
        var algoStrategyResources = await _resourceStoreService.GetAlgoStrategiesAsync();
        
        var algoStrategyResource = algoStrategyResources.Find(x => x.Id == backtestResult.StrategyId);
                
        if (algoStrategyResource is null)
            return (null, null);
                
        if (!algoStrategyResource.Enable)
            return (null, null);

        var strategy = StrategyDictionary[backtestResult.StrategyId];
        
        strategy.StabilizationPeriod = algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1;
        strategy.StartMoney = algoConfigResource.MoneyManagementResource.Money;
        strategy.EndMoney = algoConfigResource.MoneyManagementResource.Money;
        strategy.PercentOfMoney = algoConfigResource.MoneyManagementResource.PercentOfMoney;
        strategy.Ticker = backtestResult.Ticker;
        strategy.StabilizationPeriod = algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1;
        strategy.StartMoney = algoConfigResource.MoneyManagementResource.Money;
        strategy.EndMoney = algoConfigResource.MoneyManagementResource.Money;
        strategy.PercentOfMoney = algoConfigResource.MoneyManagementResource.PercentOfMoney;
        strategy.Ticker = backtestResult.Ticker;
        
        strategy.Candles = algoStrategyResource.Timeframe switch
        {
            "D" => DailyCandles.TryGetValue(strategy.Ticker, out var candles) ? candles : [],
            "H" => HourlyCandles.TryGetValue(strategy.Ticker, out var candles) ? candles : [],
            _ => []
        };
        
        if (strategy.Candles is [])
            return (null, null);
                
        if (strategy.Candles.Count < strategy.StabilizationPeriod + 1)
            return (null, null);
                
        var parameterSet = JsonSerializer.Deserialize<Dictionary<string, int>>(backtestResult.StrategyParams);
                
        if (parameterSet is null)
            return (null, null);
        
        strategy.Parameters = parameterSet;
        strategy.StopLimits.Clear();
        strategy.Positions.Clear();
        strategy.EqiutyCurve.Clear();
        strategy.DrawdownCurve.Clear();
        strategy.EndMoney = algoConfigResource.MoneyManagementResource.Money;
        
        var sw = Stopwatch.StartNew();
                    
        try
        {
            strategy.Execute();
                    
            backtestResult = CreateBacktestResult(strategy);
        }
                    
        catch (Exception exception)
        {
            _logger.Error($"Ошибка '{algoStrategyResource.Name}', '{backtestResult.StrategyId}', '{strategy.Ticker}', '{exception.Message}'");
        }
                    
        sw.Stop();
                    
        Debug.Print($"Бэктест '{algoStrategyResource.Name}', '{backtestResult.StrategyId}', '{strategy.Ticker}', '{JsonSerializer.Serialize(parameterSet)}' {sw.Elapsed.TotalMilliseconds:N2} ms");

        return (backtestResult, strategy);
    }

    public async Task<bool> CalculateStrategySignalsAsync()
    {
        var algoConfigResource = await _resourceStoreService.GetAlgoConfigAsync();
        var backtestResults = await backtestResultRepository.GetAsync(algoConfigResource.BacktestResultFilterResource);
        
        var tickersInStrategiSignals = (await strategySignalRepository.GetAllAsync()).Select(x => x.Ticker).ToList();
        var tickersInBacktestResults = backtestResults.Select(x => x.Ticker).Distinct().ToList();

        foreach (var ticker in tickersInStrategiSignals)
        {
            if (!tickersInBacktestResults.Contains(ticker))
                await strategySignalRepository.UpdatePositionAsync([ticker], 0, 0.0);
        }

        foreach (var ticker in tickersInBacktestResults)
        {
            if (!tickersInStrategiSignals.Contains(ticker))
                await strategySignalRepository.AddAsync(new StrategySignal { Ticker = ticker, CountSignals = 0 });
                
            var countLongSignals = backtestResults.Where(x => x.Ticker == ticker).Count(x => x.CurrentPosition > 0);
            var countShortSignals = backtestResults.Where(x => x.Ticker == ticker).Count(x => x.CurrentPosition < 0);
            
            await strategySignalRepository.UpdatePositionAsync(
                [ticker], 
                countLongSignals - countShortSignals,
                (countLongSignals - countShortSignals) * algoConfigResource.MoneyManagementResource.UnitSize);
        }

        return true;
    }

    private static BacktestResult CreateBacktestResult(Strategy strategy)
    {
        var json = JsonSerializer.Serialize(strategy.Parameters);
        
        var result = new BacktestResult
        {
            StrategyId = strategy.StrategyId,
            StartDate = strategy.StartDate,
            EndDate = strategy.EndDate,
            Timeframe  = strategy.Timeframe,
            Ticker  = strategy.Ticker,
            StrategyDescription  = strategy.StrategyDescription,
            StrategyName  = strategy.StrategyName,
            StrategyParams  = json,
            StrategyParamsHash = ConvertHelper.Md5Encode(json),
            NumberPositions  = strategy.NumberPositions,
            CurrentPosition  = strategy.CurrentPosition,
            CurrentPositionCost  = strategy.CurrentPositionCost,
            ProfitFactor  = strategy.ProfitFactor,
            RecoveryFactor  = strategy.RecoveryFactor,
            NetProfit  = strategy.NetProfit,
            AverageProfit  = strategy.AverageNetProfit,
            AverageProfitPercent  = strategy.AverageNetProfitPercent,
            Drawdown  = strategy.Drawdown,
            MaxDrawdown  = strategy.MaxDrawdown,
            MaxDrawdownPercent  = strategy.MaxDrawdownPercent,
            WinningPositions  = strategy.WinningPositions,
            WinningTradesPercent  = strategy.WinningTradesPercent,
            StartMoney  = strategy.StartMoney,
            EndMoney  = strategy.EndMoney,
            TotalReturn  = strategy.TotalReturn,
            AnnualYieldReturn  = strategy.AnnualYieldReturn
        };
        
        return result;
    }
}