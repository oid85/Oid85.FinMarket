using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using NLog;
using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;
using Candle = Oid85.FinMarket.Domain.Models.Algo.Candle;

namespace Oid85.FinMarket.Application.Services;

public class AlgoService(
    ILogger logger,
    IResourceStoreService resourceStoreService,
    IBacktestResultRepository backtestResultRepository,
    IOptimizationResultRepository optimizationResultRepository,
    IStrategySignalRepository strategySignalRepository,
    IShareRepository shareRepository,
    IFutureRepository futureRepository,
    ITickerListUtilService tickerListUtilService,
    AlgoHelper algoHelper)
    : IAlgoService
{
    private ConcurrentDictionary<string, List<Candle>> Candles { get; set; } = new();
    private ConcurrentDictionary<Guid, Strategy> Strategies { get; set; } = new();
    
    /// <inheritdoc />
    public async Task<bool> BacktestAsync()
    {
        Candles = new ConcurrentDictionary<string, List<Candle>>(await algoHelper.GetAlgoCandlesAsync(false));
        Strategies = new ConcurrentDictionary<Guid, Strategy>(await algoHelper.GetAlgoStrategies());
        
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var algoStrategyResources = await resourceStoreService.GetAlgoStrategiesAsync();

        var optimizationResults = await optimizationResultRepository.GetAsync(algoConfigResource.OptimizationResultFilterResource);
        
        await backtestResultRepository.InvertDeleteAsync(algoStrategyResources.Select(x => x.Id).ToList());
        
        foreach (var (strategyId, strategy) in Strategies)
        {
            await backtestResultRepository.DeleteAsync(strategyId);
            
            var algoStrategyResource = algoStrategyResources.Find(x => x.Id == strategyId);
                
            if (algoStrategyResource is null)
                continue;
            
            var backtestResults = new List<BacktestResult>();
            
            foreach (var optimizationResult in optimizationResults.Where(x => x.StrategyId == strategyId))
            {
                try
                {
                    strategy.Ticker = optimizationResult.Ticker;
                    strategy.IsFuture = await algoHelper.IsFuture(optimizationResult.Ticker);
                    strategy.BasicAssetSize = await algoHelper.GetBasicAssetSize(optimizationResult.Ticker);
                    strategy.Candles = Candles.TryGetValue(strategy.Ticker, out var candles) ? candles : [];

                    if (strategy.Candles is [])
                        continue;

                    var parameterSet = JsonSerializer.Deserialize<Dictionary<string, int>>(optimizationResult.StrategyParams);

                    if (parameterSet is null)
                        continue;
                    
                    strategy.InitForParameterSet(
                        parameterSet, 
                        algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1, 
                        algoConfigResource.MoneyManagementResource.Money, 
                        algoConfigResource.MoneyManagementResource.Money);
                    
                    strategy.Execute();
                    
                    backtestResults.Add(AlgoMapper.MapToBacktestResult(strategy));
                }
                
                catch (Exception exception)
                {
                    logger.Info($"Бэктест '{optimizationResult.Ticker}', '{optimizationResult.StrategyName}', '{optimizationResult.StrategyParams}'. {exception}");
                }  
            }
            
            await backtestResultRepository.AddAsync(backtestResults);
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<(BacktestResult? backtestResult, Strategy? strategy)> BacktestAsync(Guid backtestResultId)
    {
        try
        {
            var backtestResult = await backtestResultRepository.GetAsync(backtestResultId);

            if (backtestResult is null)
                return (null, null);
            
            Candles = new ConcurrentDictionary<string, List<Candle>>(await algoHelper.GetAlgoCandlesAsync(false, backtestResult.Ticker));
            Strategies = new ConcurrentDictionary<Guid, Strategy>(await algoHelper.GetAlgoStrategies(backtestResult.StrategyId));
            
            var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
            var algoStrategyResources = await resourceStoreService.GetAlgoStrategiesAsync();

            var algoStrategyResource = algoStrategyResources.Find(x => x.Id == backtestResult.StrategyId);

            if (algoStrategyResource is null)
                return (null, null);

            var strategy = Strategies[backtestResult.StrategyId];
            
            strategy.Ticker = backtestResult.Ticker;
            strategy.IsFuture = await algoHelper.IsFuture(backtestResult.Ticker);
            strategy.BasicAssetSize = await algoHelper.GetBasicAssetSize(backtestResult.Ticker);
            strategy.Candles = Candles.TryGetValue(strategy.Ticker, out var candles) ? candles : [];

            if (strategy.Candles is [])
                return (null, null);

            var parameterSet = JsonSerializer.Deserialize<Dictionary<string, int>>(backtestResult.StrategyParams);

            if (parameterSet is null)
                return (null, null);

            strategy.InitForParameterSet(
                parameterSet, 
                algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1, 
                algoConfigResource.MoneyManagementResource.Money, 
                algoConfigResource.MoneyManagementResource.Money);            
            
            strategy.Execute();
            
            return (AlgoMapper.MapToBacktestResult(strategy), strategy);
        }
        
        catch (Exception exception)
        {
            logger.Info($"Бэктест '{backtestResultId}'. {exception}");
            return (null, null);
        }
    }

    /// <inheritdoc />
    public async Task<bool> CalculateStrategySignalsAsync()
    {
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var algoStrategyResources = await resourceStoreService.GetAlgoStrategiesAsync();
        
        var backtestResults = await backtestResultRepository.GetAsync(algoConfigResource.BacktestResultFilterResource);
        
        // Добавляем тиекры, если их еще нет в таблице
        var tickersInStrategySignals = (await strategySignalRepository.GetAllAsync()).Select(x => x.Ticker).ToList();
        var tickersInBacktestResults = backtestResults.Select(x => x.Ticker).Distinct().ToList();
        
        foreach (var ticker in tickersInStrategySignals)
            if (!tickersInBacktestResults.Contains(ticker))
                await strategySignalRepository.UpdatePositionAsync(
                    new StrategySignal(ticker));

        // Расчет для каждого тикера
        foreach (var ticker in tickersInBacktestResults)
        {
            try
            {
                if (!tickersInStrategySignals.Contains(ticker))
                    await strategySignalRepository.AddAsync(
                        new StrategySignal(ticker));
                
                // Количество сигналов
                int countSignals = GetCountSignals(ticker);
                
                // Количество стратегий
                int countStrategies = backtestResults.Count(x => x.Ticker == ticker);
                
                // Процент сигналов
                double percentSignals = Convert.ToDouble(countSignals) / Convert.ToDouble(countStrategies) * 100.0;
                
                // Количество уникальных тикеров с позицией не равной 0
                int countUniqueTickersWithSignals = backtestResults
                    .Where(x => x.CurrentPosition is > 0 or < 0)
                    .Select(x => x.Ticker).Distinct().Count();
                
                // Размер позиции в процентах от портфеля
                double positionPercentPortfolio = ((algoConfigResource.MoneyManagementResource.Money / countUniqueTickersWithSignals) * (percentSignals / 100.0)) / algoConfigResource.MoneyManagementResource.Money * 100.0;
                
                // Размер позиции, руб
                double positionCost = algoConfigResource.MoneyManagementResource.Money * positionPercentPortfolio / 100.0;
                
                // Цена инструмента
                double lastPrice = await GetLastPriceAsync(ticker);                
                
                // Размер позиции, шт
                double positionSize = 0;
                
                if (positionCost != 0.0 && lastPrice != 0.0)
                    positionSize = positionCost / lastPrice;
                
                // Применяем плечо
                var sharesTickers = (await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.AlgoShares)).Select(x => x.Ticker).ToList();
                var futuresTickers = (await tickerListUtilService.GetFuturesByTickerListAsync(KnownTickerLists.AlgoFutures)).Select(x => x.Ticker).ToList();

                if (sharesTickers.Contains(ticker))
                {
                    positionSize *= algoConfigResource.MoneyManagementResource.ShareLeverage;
                    positionCost *= algoConfigResource.MoneyManagementResource.ShareLeverage;
                }


                if (futuresTickers.Contains(ticker))
                {
                    positionSize *= algoConfigResource.MoneyManagementResource.FutureLeverage;
                    positionCost *= algoConfigResource.MoneyManagementResource.FutureLeverage;
                }

                // Знак позиции
                if (countSignals > 0)
                    positionSize = Math.Abs(positionSize);
                
                else if (countSignals < 0)
                    positionSize = -1 * Math.Abs(positionSize);
                
                await strategySignalRepository.UpdatePositionAsync(
                    new StrategySignal
                    {
                        Ticker = ticker, 
                        CountStrategies = Math.Abs(countStrategies), 
                        CountSignals = Math.Abs(countSignals), 
                        PercentSignals = Math.Abs(percentSignals), 
                        LastPrice = lastPrice, 
                        PositionCost = Math.Abs(positionCost), 
                        PositionSize = Convert.ToInt32(positionSize),
                        PositionPercentPortfolio = positionPercentPortfolio 
                    });
            }
            
            catch (Exception exception)
            {
                logger.Error($"Ошибка CalculateStrategySignalsAsync '{ticker}', '{exception.Message}'");
            }
        }

        return true;
        
        async Task<double> GetLastPriceAsync(string ticker)
        {
            var sharesTickers = (await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.AlgoShares)).Select(x => x.Ticker).ToList();
            var futuresTickers = (await tickerListUtilService.GetFuturesByTickerListAsync(KnownTickerLists.AlgoFutures)).Select(x => x.Ticker).ToList();
            
            if (sharesTickers.Contains(ticker))
                return (await shareRepository.GetAsync(ticker))!.LastPrice;

            if (futuresTickers.Contains(ticker))
                return (await futureRepository.GetAsync(ticker))!.LastPrice;

            return 0.0;
        }
        
        int GetCountSignals(string ticker)
        {
            int countSignals = 0;
            
            foreach (var backtestResult in backtestResults.Where(x => x.Ticker == ticker))
            {
                var enable = algoStrategyResources.Find(x => x.Id == backtestResult.StrategyId)!.Enable;
                    
                if (!enable)
                    continue;
                    
                switch (backtestResult.CurrentPosition)
                {
                    case > 0:
                        countSignals++;
                        break;
                    
                    case < 0:
                        countSignals--;
                        break;
                }
            }
            
            return countSignals;
        }
    }

    /// <inheritdoc />
    public async Task<bool> OptimizeAsync()
    {
        var sw = Stopwatch.StartNew();
        
        Candles = new ConcurrentDictionary<string, List<Candle>>(await algoHelper.GetAlgoCandlesAsync(true));
        Strategies = new ConcurrentDictionary<Guid, Strategy>(await algoHelper.GetAlgoStrategies());
        
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var algoStrategyResources = await resourceStoreService.GetAlgoStrategiesAsync();
        
        await optimizationResultRepository.InvertDeleteAsync(algoStrategyResources.Select(x => x.Id).ToList());

        int count = 0;
        
        foreach (var (strategyId, strategy) in Strategies)
        {
            await optimizationResultRepository.DeleteAsync(strategyId);
            
            var algoStrategyResource = algoStrategyResources.Find(x => x.Id == strategyId);
            
            if (algoStrategyResource is null)
                continue;

            var optimizationResults = new List<OptimizationResult>();
            
            var tickers = (await resourceStoreService.GetTickerListAsync(algoStrategyResource.TickerList)).Tickers;
            
            foreach (var ticker in tickers)
            {
                strategy.Ticker = ticker;
                strategy.IsFuture = await algoHelper.IsFuture(ticker);
                strategy.BasicAssetSize = await algoHelper.GetBasicAssetSize(ticker);
                strategy.Candles = Candles.TryGetValue(strategy.Ticker, out var candles) ? candles : [];

                if (strategy.Candles is [])
                    continue;
                
                var parameterSets = AlgoHelper.GetParameterSets(algoStrategyResource.Params);
                
                foreach (var parameterSet in parameterSets)
                {
                    try
                    {
                        if (parameterSet.Count == 0)
                            continue;
                        
                        strategy.InitForParameterSet(
                            parameterSet, 
                            algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1, 
                            algoConfigResource.MoneyManagementResource.Money, 
                            algoConfigResource.MoneyManagementResource.Money);                        
                        
                        strategy.Execute();
                        
                        optimizationResults.Add(AlgoMapper.MapToOptimizationResult(strategy));
                    }
                    
                    catch (Exception exception)
                    {
                        logger.Info($"Оптимизация '{strategy.Ticker}', '{strategy.StrategyName}', '{JsonSerializer.Serialize(parameterSet)}'. {exception}");
                    }
                }
            }
            
            await optimizationResultRepository.AddAsync(optimizationResults);

            count++;
                
            logger.Info($"Оптимизация '{strategy.StrategyName}' закончена. {count} из {Strategies.Count}");
        }
        
        sw.Stop();
        
        logger.Info($"Оптимизация закончена за {sw.Elapsed.TotalMinutes:N2} минут");
        
        return true;
    }
}