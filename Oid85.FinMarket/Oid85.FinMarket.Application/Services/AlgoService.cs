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
using Candle = Oid85.FinMarket.Domain.Models.Algo.Candle;

namespace Oid85.FinMarket.Application.Services;

public class AlgoService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
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
    public ConcurrentDictionary<string, List<Candle>> DailyCandles { get; set; } = new();
    
    public ConcurrentDictionary<Guid, Strategy> StrategyDictionary { get; set; } = new();
    
    public async Task<bool> BacktestAsync()
    {
        await InitBacktestAsync();
        
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var algoStrategyResources = await resourceStoreService.GetAlgoStrategiesAsync();

        var optimizationResults = await optimizationResultRepository.GetAsync(algoConfigResource.OptimizationResultFilterResource);
        
        await backtestResultRepository.InvertDeleteAsync(algoStrategyResources.Select(x => x.Id).ToList());
        
        foreach (var (strategyId, strategy) in StrategyDictionary)
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
                    strategy.StabilizationPeriod = algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1;
                    strategy.StartMoney = algoConfigResource.MoneyManagementResource.Money;
                    strategy.EndMoney = algoConfigResource.MoneyManagementResource.Money;
                    strategy.Ticker = optimizationResult.Ticker;

                    strategy.Candles = DailyCandles.TryGetValue(strategy.Ticker, out var candles) ? candles : [];

                    if (strategy.Candles is [])
                        continue;

                    if (strategy.Candles.Count < strategy.StabilizationPeriod + 1)
                        continue;

                    var parameterSet =
                        JsonSerializer.Deserialize<Dictionary<string, int>>(optimizationResult.StrategyParams);

                    if (parameterSet is null)
                        continue;

                    strategy.Parameters = parameterSet;
                    strategy.StopLimits.Clear();
                    strategy.Positions.Clear();
                    strategy.EqiutyCurve.Clear();
                    strategy.DrawdownCurve.Clear();
                    strategy.EndMoney = algoConfigResource.MoneyManagementResource.Money;

                    strategy.GraphPoints.Clear();
                    for (int i = 0; i < strategy.Candles.Count; i++)
                        strategy.GraphPoints.Add(new GraphPoint());
                    
                    strategy.Execute();

                    var backtestResult = AlgoMapper.MapToBacktestResult(strategy);
                    backtestResults.Add(backtestResult);
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

    public async Task<(BacktestResult? backtestResult, Strategy? strategy)> BacktestAsync(Guid backtestResultId)
    {
        try
        {
            var backtestResult = await backtestResultRepository.GetAsync(backtestResultId);

            if (backtestResult is null)
                return (null, null);

            await InitBacktestAsync(backtestResult.Ticker, backtestResult.StrategyId);

            var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
            var algoStrategyResources = await resourceStoreService.GetAlgoStrategiesAsync();

            var algoStrategyResource = algoStrategyResources.Find(x => x.Id == backtestResult.StrategyId);

            if (algoStrategyResource is null)
                return (null, null);

            var strategy = StrategyDictionary[backtestResult.StrategyId];

            strategy.StabilizationPeriod = algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1;
            strategy.StartMoney = algoConfigResource.MoneyManagementResource.Money;
            strategy.EndMoney = algoConfigResource.MoneyManagementResource.Money;
            strategy.Ticker = backtestResult.Ticker;

            strategy.Candles = DailyCandles.TryGetValue(strategy.Ticker, out var candles) ? candles : [];

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

            strategy.GraphPoints.Clear();
            for (int i = 0; i < strategy.Candles.Count; i++)
                strategy.GraphPoints.Add(new GraphPoint());
            
            strategy.Execute();

            backtestResult = AlgoMapper.MapToBacktestResult(strategy);
            
            return (backtestResult, strategy);
        }
        
        catch (Exception exception)
        {
            logger.Info($"Бэктест '{backtestResultId}'. {exception}");
            return (null, null);
        }
    }

    public async Task<bool> CalculateStrategySignalsAsync()
    {
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var algoStrategyResources = await resourceStoreService.GetAlgoStrategiesAsync();
        
        var backtestResults = await backtestResultRepository.GetAsync(algoConfigResource.BacktestResultFilterResource);
        
        // Добавляем тиекры, если их еще нет в таблице
        var tickersInStrategySignals = (await strategySignalRepository.GetAllAsync()).Select(x => x.Ticker).ToList();
        var tickersInBacktestResults = backtestResults.Select(x => x.Ticker).Distinct().ToList();
        foreach (var ticker in tickersInStrategySignals)
        {
            if (!tickersInBacktestResults.Contains(ticker))
                await strategySignalRepository.UpdatePositionAsync(
                    new StrategySignal
                    {
                        Ticker = ticker, 
                        CountStrategies = 0, 
                        CountSignals = 0, 
                        PercentSignals = 0, 
                        LastPrice = 0.0, 
                        PositionCost = 0.0, 
                        PositionSize = 0,
                        PositionPercentPortfolio = 0 
                    });
        }

        // Расчет для каждого тикера
        foreach (var ticker in tickersInBacktestResults)
        {
            try
            {
                if (!tickersInStrategySignals.Contains(ticker))
                    await strategySignalRepository.AddAsync(
                        new StrategySignal
                        {
                            Ticker = ticker, 
                            CountStrategies = 0, 
                            CountSignals = 0, 
                            PercentSignals = 0, 
                            LastPrice = 0.0, 
                            PositionCost = 0.0, 
                            PositionSize = 0,
                            PositionPercentPortfolio = 0 
                        });
                
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
            var sharesTickers = (await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.AlgoShares))
                .Select(x => x.Ticker).ToList();
            var futuresTickers = (await tickerListUtilService.GetFuturesByTickerListAsync(KnownTickerLists.AlgoFutures))
                .Select(x => x.Ticker).ToList();
            
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

    public async Task<bool> OptimizeAsync()
    {
        var sw = Stopwatch.StartNew();
        
        await InitOptimizationAsync();
        
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var algoStrategyResources = await resourceStoreService.GetAlgoStrategiesAsync();
        
        await optimizationResultRepository.InvertDeleteAsync(algoStrategyResources.Select(x => x.Id).ToList());

        int count = 0;
        
        foreach (var (strategyId, strategy) in StrategyDictionary)
        {
            await optimizationResultRepository.DeleteAsync(strategyId);
            
            var algoStrategyResource = algoStrategyResources.Find(x => x.Id == strategyId);
            
            if (algoStrategyResource is null)
                continue;

            var optimizationResults = new List<OptimizationResult>();
            
            var tickers = (await resourceStoreService.GetTickerListAsync(algoStrategyResource.TickerList)).Tickers;
            
            foreach (var ticker in tickers)
            {
                strategy.StabilizationPeriod = algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1;
                strategy.StartMoney = algoConfigResource.MoneyManagementResource.Money;
                strategy.EndMoney = algoConfigResource.MoneyManagementResource.Money;
                strategy.Ticker = ticker;                
                
                strategy.Candles = DailyCandles.TryGetValue(strategy.Ticker, out var candles) ? candles : [];

                if (strategy.Candles is [])
                    continue;
                
                if (strategy.Candles.Count < strategy.StabilizationPeriod + 1)
                    continue;
                
                var parameterSets = algoHelper.GetParameterSets(algoStrategyResource.Params);
                
                foreach (var parameterSet in parameterSets)
                {
                    try
                    {
                        if (parameterSet.Count == 0)
                            continue;

                        strategy.Parameters = parameterSet;
                        strategy.StopLimits.Clear();
                        strategy.Positions.Clear();
                        strategy.EqiutyCurve.Clear();
                        strategy.DrawdownCurve.Clear();
                        strategy.EndMoney = algoConfigResource.MoneyManagementResource.Money;

                        strategy.GraphPoints.Clear();
                        for (int i = 0; i < strategy.Candles.Count; i++)
                            strategy.GraphPoints.Add(new GraphPoint());
                        
                        strategy.Execute();

                        var optimizationResult = AlgoMapper.MapToOptimizationResult(strategy);
                        optimizationResults.Add(optimizationResult);
                    }
                    
                    catch (Exception exception)
                    {
                        logger.Info($"Оптимизация '{strategy.Ticker}', '{strategy.StrategyName}', '{JsonSerializer.Serialize(parameterSet)}'. {exception}");
                    }
                }
            }
            
            await optimizationResultRepository.AddAsync(optimizationResults);

            count++;
                
            logger.Info($"Оптимизация '{strategy.StrategyName}' закончена. {count} из {StrategyDictionary.Count}");
        }
        
        sw.Stop();
        
        logger.Info($"Оптимизация закончена за {sw.Elapsed.TotalMinutes:N2} минут");
        
        return true;
    }
    
    private async Task InitBacktestAsync(string? ticker = null, Guid? strategyId = null)
    {
        await InitDailyCandlesAsync(false, ticker);

        StrategyDictionary = new ConcurrentDictionary<Guid, Strategy>(await algoHelper.GetAlgoStrategies(strategyId));
    }
    
    private async Task InitOptimizationAsync()
    {
        await InitDailyCandlesAsync(true);

        StrategyDictionary = new ConcurrentDictionary<Guid, Strategy>(await algoHelper.GetAlgoStrategies());
    }

    private async Task InitDailyCandlesAsync(bool isOptimization, string? ticker = null)
    {
        var dates = await GetDatesAsync(isOptimization);

        var tickers = ticker is null ? await algoHelper.GetAllTickersForAlgoAsync() : [ticker];
        
        foreach (string instrumentTicker in tickers)
        {
            var candles = (await dailyCandleRepository.GetAsync(instrumentTicker, dates.From, dates.To))
                .Select(AlgoMapper.Map).ToList();
            
            if (candles.Count == 0)
                continue;

            for (int i = 0; i < candles.Count; i++)
                candles[i].Index = i;
            
            DailyCandles.TryAdd(instrumentTicker, candles);
        }
    }
    
    private async Task<(DateOnly From, DateOnly To)> GetDatesAsync(bool isOptimization) => 
        isOptimization ? await algoHelper.GetOptimizationDatesAsync() : await algoHelper.GetBacktestDatesAsync();
}