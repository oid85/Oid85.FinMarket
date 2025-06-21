using System.Diagnostics;
using System.Text.Json;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services.Algo;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.Application.Services.Algo;

public class OptimizationService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IHourlyCandleRepository hourlyCandleRepository,
    IResourceStoreService resourceStoreService,
    IOptimizationResultRepository optimizationResultRepository,
    IServiceProvider serviceProvider) 
    : AlgoService(
        logger,
        dailyCandleRepository,
        hourlyCandleRepository,
        resourceStoreService,
        serviceProvider), 
        IOptimizationService
{
    private readonly IResourceStoreService _resourceStoreService = resourceStoreService;
    private readonly ILogger _logger = logger;

    public async Task<bool> OptimizeAsync()
    {
        await InitOptimizationAsync();
        
        var algoConfigResource = await _resourceStoreService.GetAlgoConfigAsync();
        var algoStrategyResources = await _resourceStoreService.GetAlgoStrategiesAsync();
        
        await optimizationResultRepository.InvertDeleteAsync(algoStrategyResources.Select(x => x.Id).ToList());
        
        foreach (var (strategyId, strategy) in StrategyDictionary)
        {
            await optimizationResultRepository.DeleteAsync(strategyId);
            
            var algoStrategyResource = algoStrategyResources.Find(x => x.Id == strategyId);
            
            if (algoStrategyResource is null)
                continue;
                
            if (!algoStrategyResource.Enable)
                continue;
            
            var optimizationResults = new List<OptimizationResult>();
            
            var tickers = (await _resourceStoreService.GetTickerListAsync(algoStrategyResource.TickerList)).Tickers;
            
            foreach (var ticker in tickers)
            {
                strategy.StabilizationPeriod = algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1;
                strategy.StartMoney = algoConfigResource.MoneyManagementResource.Money;
                strategy.EndMoney = algoConfigResource.MoneyManagementResource.Money;
                strategy.PercentOfMoney = algoConfigResource.MoneyManagementResource.PercentOfMoney;
                strategy.Ticker = ticker;                
                
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
                
                var parameterSets = GetParameterSets(algoStrategyResource.Params);

                var sw = Stopwatch.StartNew();
                
                foreach (var parameterSet in parameterSets)
                {
                    if (parameterSet.Count == 0) 
                        continue;
                    
                    strategy.Parameters = parameterSet;
                    strategy.StopLimits.Clear();
                    strategy.Positions.Clear();
                    strategy.EqiutyCurve.Clear();
                    strategy.DrawdownCurve.Clear();
                    strategy.EndMoney = algoConfigResource.MoneyManagementResource.Money;
                    
                    try
                    {
                        strategy.Execute();
                        
                        var optimizationResult = CreateOptimizationResult(strategy);
                        optimizationResults.Add(optimizationResult);
                    }
                    
                    catch (Exception exception)
                    {
                        _logger.Error($"Ошибка '{algoStrategyResource.Name}', '{strategyId}', '{ticker}', '{exception.Message}'");
                    }
                }
                
                sw.Stop();
                    
                Debug.Print($"Оптимизация '{algoStrategyResource.Name}', '{strategyId}', '{ticker}' {sw.Elapsed.TotalMilliseconds:N2} ms");
            }
            
            await optimizationResultRepository.AddAsync(optimizationResults);
        }
        
        return true;
    }

    private static List<Dictionary<string, int>> GetParameterSets(List<StrategyParamResource> strategyParams)
    {
        var result = new List<Dictionary<string, int>>();
        
        switch (strategyParams.Count)
        {
            case 1:
                for (int paramValue1 = strategyParams[0].Min; paramValue1 <= strategyParams[0].Max; paramValue1 += strategyParams[0].Step) 
                    result.Add(
                        new Dictionary<string, int>
                        {
                            [strategyParams[0].Name] = paramValue1
                        });

                return result;
            
            case 2:
                for (int paramValue1 = strategyParams[0].Min; paramValue1 <= strategyParams[0].Max; paramValue1 += strategyParams[0].Step) 
                for (int paramValue2 = strategyParams[1].Min; paramValue2 <= strategyParams[1].Max; paramValue2 += strategyParams[1].Step) 
                    result.Add(
                        new Dictionary<string, int>
                        {
                            [strategyParams[0].Name] = paramValue1,
                            [strategyParams[1].Name] = paramValue2
                        });

                return result;
            
            case 3:
                for (int paramValue1 = strategyParams[0].Min; paramValue1 <= strategyParams[0].Max; paramValue1 += strategyParams[0].Step) 
                for (int paramValue2 = strategyParams[1].Min; paramValue2 <= strategyParams[1].Max; paramValue2 += strategyParams[1].Step)
                for (int paramValue3 = strategyParams[2].Min; paramValue3 <= strategyParams[2].Max; paramValue3 += strategyParams[2].Step)
                    result.Add(
                        new Dictionary<string, int>
                        {
                            [strategyParams[0].Name] = paramValue1,
                            [strategyParams[1].Name] = paramValue2,
                            [strategyParams[2].Name] = paramValue3
                        });
                
                return result;     
        }

        throw new Exception("Количество параметров больше 3. Оптимизация выполняться не будет");
    }

    private static OptimizationResult CreateOptimizationResult(Strategy strategy)
    {
        var json = JsonSerializer.Serialize(strategy.Parameters);
        
        var result = new OptimizationResult
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