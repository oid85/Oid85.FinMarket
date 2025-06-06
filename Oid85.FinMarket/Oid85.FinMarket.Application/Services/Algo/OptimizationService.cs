﻿using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services.Algo;
using Oid85.FinMarket.External.ResourceStore;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.Application.Services.Algo;

public class OptimizationService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IHourlyCandleRepository hourlyCandleRepository,
    IResourceStoreService resourceStoreService,
    IServiceProvider serviceProvider) 
    : AlgoEngine(
        logger,
        dailyCandleRepository,
        hourlyCandleRepository,
        resourceStoreService,
        serviceProvider), 
        IOptimizationService
{
    private readonly IResourceStoreService _resourceStoreService = resourceStoreService;

    public async Task<bool> OptimizeAsync()
    {
        await InitOptimizationAsync();
        
        var algoConfigResource = await _resourceStoreService.GetAlgoConfigAsync();
        var algoStrategyResources = await _resourceStoreService.GetAlgoStrategiesAsync();

        foreach (var item in StrategyDictionary)
        {
            var strategy  = item.Value;
            
            foreach (var ticker in algoConfigResource.Tickers)
            {
                var algoStrategyResource = algoStrategyResources.Find(x => x.Id == item.Key);
                
                if (algoStrategyResource is null)
                    continue;
                
                if (!algoStrategyResource.Enable)
                    continue;

                strategy.Candles = algoStrategyResource.Timeframe switch
                {
                    "D" => DailyCandles[ticker],
                    "H" => HourlyCandles[ticker],
                    _ => []
                };

                if (strategy.Candles is [])
                    continue;
                
                var parameterSets = GetParameterSets(algoStrategyResource.Params);

                foreach (var parameterSet in parameterSets)
                {
                    if (parameterSet.Count == 0) 
                        continue;
                    
                    strategy.Parameters = parameterSet;
                    strategy.Execute();
                }
            }
        }
        
        return true;
    }

    private static List<Dictionary<string, int>> GetParameterSets(List<StrategyParam> strategyParams)
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
}