using NLog;
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

                var parameterSets = GetParameterSets(algoStrategyResource.Params);

                foreach (var parameterSet in parameterSets)
                {
                    strategy.Parameters = parameterSet;
                    strategy.Execute();
                }
            }
        }
        
        return true;
    }

    private static List<Dictionary<string, int>> GetParameterSets(List<StrategyParam> strategyParams)
    {
        switch (strategyParams.Count)
        {
            case 1:
                break;
            
            case 2:
                break;
            
            case 3:
                break;            
        }

        throw new Exception("Количество параметров больше 3. Оптимизация выполняться не будет");
    }
}