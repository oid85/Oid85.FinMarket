using Microsoft.Extensions.DependencyInjection;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.Application.Helpers;

public class AlgoHelper(
    IServiceProvider serviceProvider,
    IResourceStoreService resourceStoreService,
    IRegressionTailRepository regressionTailRepository)
{
    /// <summary>
    /// Получить даты для оптимизации
    /// </summary>
    public async Task<(DateOnly From, DateOnly To)> GetOptimizationDatesAsync()
    {
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();

        var today = DateOnly.FromDateTime(DateTime.Today);
        
        var from = today
            .AddDays(-1 * algoConfigResource.PeriodConfigResource.BacktestWindowInDays)
            .AddDays(-1 * algoConfigResource.PeriodConfigResource.DailyStabilizationPeriodInDays)
            .AddDays(-1 * algoConfigResource.PeriodConfigResource.BacktestShiftInDays);
            
        var to = today.AddDays(-1 * algoConfigResource.PeriodConfigResource.BacktestShiftInDays);

        return (from, to);
    }
    
    /// <summary>
    /// Получить даты для бэктеста
    /// </summary>
    public async Task<(DateOnly From, DateOnly To)> GetBacktestDatesAsync()
    {
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();

        var today = DateOnly.FromDateTime(DateTime.Today);
        
        var from = today
            .AddDays(-1 * algoConfigResource.PeriodConfigResource.BacktestWindowInDays)
            .AddDays(-1 * algoConfigResource.PeriodConfigResource.DailyStabilizationPeriodInDays);
            
        var to = today;

        return (from, to);
    }   
    
    /// <summary>
    /// Получить тикеры для Алго
    /// </summary>
    public async Task<List<string>> GetAllTickersForAlgoAsync()
    {
        var algoStrategyResources = await resourceStoreService.GetAlgoStrategiesAsync();
        
        var tickers = new List<string>();

        foreach (var algoStrategyResource in algoStrategyResources)
        {
            var tickersInTickerList = (await resourceStoreService.GetTickerListAsync(algoStrategyResource.TickerList)).Tickers;

            foreach (var ticker in tickersInTickerList.Where(ticker => !tickers.Contains(ticker))) 
                tickers.Add(ticker);
        }
        
        return tickers;
    }    
    
    /// <summary>
    /// Получить тикеры для Статистического арбитража
    /// </summary>
    public async Task<List<string>> GetAllTickersForStatisticalArbitrageAsync()
    {
        var tails = await regressionTailRepository.GetAllAsync();
        
        List<string> tickers = 
                [
                    ..tails.Select(x => x.Ticker1).ToList(),
                    ..tails.Select(x => x.Ticker2).ToList()
                ];
        
        return tickers.Distinct().ToList();
    }
    
    /// <summary>
    /// Получить стратегии для Алго
    /// </summary>
    /// <param name="strategyId">Id стратегии</param>
    public async Task<Dictionary<Guid, Strategy>> GetAlgoStrategies(Guid? strategyId = null)
    {
        var algoStrategyResources = strategyId is null 
            ? await resourceStoreService.GetAlgoStrategiesAsync() 
            : (await resourceStoreService.GetAlgoStrategiesAsync()).Where(x => x.Id == strategyId);
        
        var strategyDictionary = new Dictionary<Guid, Strategy>();
        
        foreach (var algoStrategyResource in algoStrategyResources)
        {
            var strategy = serviceProvider.GetRequiredKeyedService<Strategy>(algoStrategyResource.Name);
                
            strategy.StrategyId = algoStrategyResource.Id;
            strategy.Timeframe = algoStrategyResource.Timeframe;
            strategy.StrategyDescription = algoStrategyResource.Description;
            strategy.StrategyName = algoStrategyResource.Name;
                
            strategyDictionary.TryAdd(algoStrategyResource.Id, strategy);
        }

        return strategyDictionary;
    }
    
    /// <summary>
    /// Получить стратегии для Статистического арбитража
    /// </summary>
    /// <param name="strategyId">Id стратегии</param>
    public async Task<Dictionary<Guid, StatisticalArbitrageStrategy>> GetStatisticalArbitrageStrategies(Guid? strategyId = null)
    {
        var algoStrategyResources = strategyId is null
            ? await resourceStoreService.GetAlgoStrategiesAsync() 
            : (await resourceStoreService.GetAlgoStrategiesAsync()).Where(x => x.Id == strategyId);

        var strategyDictionary = new Dictionary<Guid, StatisticalArbitrageStrategy>();        
        
        foreach (var algoStrategyResource in algoStrategyResources)
        {
            var strategy = serviceProvider.GetRequiredKeyedService<StatisticalArbitrageStrategy>(algoStrategyResource.Name);

            strategy.StrategyId = algoStrategyResource.Id;
            strategy.Timeframe = algoStrategyResource.Timeframe;
            strategy.StrategyDescription = algoStrategyResource.Description;
            strategy.StrategyName = algoStrategyResource.Name;

            strategyDictionary.TryAdd(algoStrategyResource.Id, strategy);
        }
        
        return strategyDictionary;
    }  
    
    /// <summary>
    /// Получить параметры стратегии
    /// </summary>
    /// <param name="strategyParams">Параметры из ресурсов</param>
    public List<Dictionary<string, int>> GetParameterSets(List<StrategyParamResource> strategyParams)
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