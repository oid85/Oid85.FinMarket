using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using NLog;
using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Common.Utils;
using Oid85.FinMarket.Domain.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.Computation;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services;

public class AlgoStatisticalArbitrageService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IResourceStoreService resourceStoreService,
    IStatisticalArbitrageBacktestResultRepository backtestResultRepository,
    IStatisticalArbitrageOptimizationResultRepository optimizationResultRepository,
    IStatisticalArbitrageStrategySignalRepository strategySignalRepository,
    IShareRepository shareRepository,
    IFutureRepository futureRepository,
    ICorrelationRepository correlationRepository,
    IRegressionTailRepository regressionTailRepository,
    ITickerListUtilService tickerListUtilService,
    AlgoHelper algoHelper)
    : IAlgoStatisticalArbitrageService
{
    private ConcurrentDictionary<string, List<Candle>> Candles { get; set; } = new();
    private ConcurrentDictionary<string, RegressionTail> Spreads { get; set; } = new();
    private ConcurrentDictionary<Guid, StatisticalArbitrageStrategy> Strategies { get; set; } = new();

    /// <inheritdoc />
    public async Task<bool> BacktestAsync()
    {
        Spreads = new ConcurrentDictionary<string, RegressionTail>(await algoHelper.GetSpreadsAsync());
        Candles = new ConcurrentDictionary<string, List<Candle>>(await algoHelper.GetStatisticalArbitrageCandlesAsync(false));
        Strategies = new ConcurrentDictionary<Guid, StatisticalArbitrageStrategy>(await algoHelper.GetStatisticalArbitrageStrategies());
        
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var statisticalArbitrageStrategyResources = await resourceStoreService.GetStatisticalArbitrageStrategiesAsync();

        var optimizationResults = await optimizationResultRepository.GetAsync(algoConfigResource.OptimizationResultFilterResource);

        await backtestResultRepository.InvertDeleteAsync(statisticalArbitrageStrategyResources.Select(x => x.Id).ToList());

        foreach (var (strategyId, strategy) in Strategies)
        {
            await backtestResultRepository.DeleteAsync(strategyId);

            var statisticalArbitrageStrategyResource = statisticalArbitrageStrategyResources.Find(x => x.Id == strategyId);

            if (statisticalArbitrageStrategyResource is null)
                continue;

            var backtestResults = new List<StatisticalArbitrageBacktestResult>();

            foreach (var optimizationResult in optimizationResults.Where(x => x.StrategyId == strategyId))
            {
                try
                {
                    strategy.Ticker = (optimizationResult.TickerFirst, optimizationResult.TickerSecond);

                    var syncCandles = algoHelper.SyncCandles(
                        Candles.TryGetValue(strategy.Ticker.First, out var candles1) ? candles1 : [], 
                        Candles.TryGetValue(strategy.Ticker.First, out var candles2) ? candles2 : []);
                    
                    strategy.Candles = (syncCandles.Candles1, syncCandles.Candles2);

                    if (strategy.Candles.First is [] || strategy.Candles.Second is [])
                        continue;

                    var tail = await regressionTailRepository.GetAsync(strategy.Ticker.First, strategy.Ticker.Second);
                    
                    if (tail is null)
                        continue;

                    strategy.Spreads = tail.Tails;
                    
                    var parameterSet = JsonSerializer.Deserialize<Dictionary<string, int>>(optimizationResult.StrategyParams);

                    if (parameterSet is null)
                        continue;

                    strategy.InitForParameterSet(
                        parameterSet, 
                        algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1, 
                        algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney, 
                        algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney); 

                    await strategy.Execute();
                    
                    backtestResults.Add(AlgoMapper.MapToBacktestResult(strategy));
                }

                catch (Exception exception)
                {
                    logger.Info($"Бэктест '{optimizationResult.TickerFirst}, {optimizationResult.TickerSecond}', '{optimizationResult.StrategyName}', '{optimizationResult.StrategyParams}'. {exception}");
                }
            }

            await backtestResultRepository.AddAsync(backtestResults);
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<(StatisticalArbitrageBacktestResult? backtestResult, StatisticalArbitrageStrategy? strategy)> BacktestAsync(Guid backtestResultId)
    {
        try
        {
            var backtestResult = await backtestResultRepository.GetAsync(backtestResultId);

            if (backtestResult is null)
                return (null, null);
            
            Spreads = new ConcurrentDictionary<string, RegressionTail>(await algoHelper.GetSpreadsAsync(backtestResult.TickerFirst, backtestResult.TickerSecond));
            Candles = new ConcurrentDictionary<string, List<Candle>>(await algoHelper.GetStatisticalArbitrageCandlesAsync(false, backtestResult.TickerFirst, backtestResult.TickerSecond));
            Strategies = new ConcurrentDictionary<Guid, StatisticalArbitrageStrategy>(await algoHelper.GetStatisticalArbitrageStrategies(backtestResult.StrategyId));
            
            var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
            var statisticalArbitrageStrategyResources = await resourceStoreService.GetStatisticalArbitrageStrategiesAsync();

            var statisticalArbitrageStrategyResource = statisticalArbitrageStrategyResources.Find(x => x.Id == backtestResult.StrategyId);

            if (statisticalArbitrageStrategyResource is null)
                return (null, null);

            var strategy = Strategies[backtestResult.StrategyId];
            
            strategy.Ticker = (backtestResult.TickerFirst, backtestResult.TickerSecond);
            
            var syncCandles = algoHelper.SyncCandles(
                Candles.TryGetValue(strategy.Ticker.First, out var candles1) ? candles1 : [], 
                Candles.TryGetValue(strategy.Ticker.First, out var candles2) ? candles2 : []);
                    
            strategy.Candles = (syncCandles.Candles1, syncCandles.Candles2);

            if (strategy.Candles.First is [] || strategy.Candles.Second is [] )
                return (null, null);

            var tail = await regressionTailRepository.GetAsync(strategy.Ticker.First, strategy.Ticker.Second);
                    
            if (tail is null)
                return (null, null);

            strategy.Spreads = tail.Tails;
            
            var parameterSet = JsonSerializer.Deserialize<Dictionary<string, int>>(backtestResult.StrategyParams);

            if (parameterSet is null)
                return (null, null);

            strategy.InitForParameterSet(
                parameterSet, 
                algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1, 
                algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney, 
                algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney); 

            await strategy.Execute();
            
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
        var statisticalArbitrageStrategyResources = await resourceStoreService.GetStatisticalArbitrageStrategiesAsync();

        var backtestResults = await backtestResultRepository.GetAsync(algoConfigResource.BacktestResultFilterResource);

        // Добавляем тикеры, если их еще нет в таблице
        var tickersInStrategySignals = (await strategySignalRepository.GetAllAsync()).Select(x => $"{x.TickerFirst},{x.TickerSecond}").ToList();
        var tickersInBacktestResults = backtestResults.Select(x => $"{x.TickerFirst},{x.TickerSecond}").Distinct().ToList();
        foreach (var tickerPair in tickersInStrategySignals)
        {
            if (!tickersInBacktestResults.Contains(tickerPair))
                await strategySignalRepository.UpdatePositionAsync(
                    new StatisticalArbitrageStrategySignal
                    {
                        TickerFirst = tickerPair.Split(',')[0],
                        TickerSecond = tickerPair.Split(',')[1],
                        CountStrategies = 0,
                        CountSignals = 0,
                        PercentSignals = 0,
                        LastPriceFirst = 0.0,
                        LastPriceSecond = 0.0,
                        PositionCost = 0.0,
                        PositionSizeFirst = 0,
                        PositionSizeSecond = 0,
                        PositionPercentPortfolio = 0
                    });
        }

        // Расчет для каждого тикера
        foreach (var tickerPair in tickersInBacktestResults)
        {
            try
            {
                if (!tickersInStrategySignals.Contains(tickerPair))
                    await strategySignalRepository.AddAsync(
                        new StatisticalArbitrageStrategySignal
                        {
                            TickerFirst = tickerPair.Split(',')[0],
                            TickerSecond = tickerPair.Split(',')[1],
                            CountStrategies = 0,
                            CountSignals = 0,
                            PercentSignals = 0,
                            LastPriceFirst = 0.0,
                            LastPriceSecond = 0.0,
                            PositionCost = 0.0,
                            PositionSizeFirst = 0,
                            PositionSizeSecond = 0,
                            PositionPercentPortfolio = 0
                        });

                var backtestResultsByTickerPair = backtestResults.Where(x => $"{x.TickerFirst},{x.TickerSecond}" == tickerPair).ToList();
                
                // Количество сигналов
                int countSignals = GetCountSignals(tickerPair);

                // Количество стратегий
                int countStrategies = backtestResultsByTickerPair.Count;

                // Процент сигналов
                double percentSignals = Convert.ToDouble(countSignals) / Convert.ToDouble(countStrategies) * 100.0;

                // Количество уникальных тикеров с позицией не равной 0
                int countUniqueTickersWithSignals = backtestResults
                    .Where(
                        x => 
                            x.CurrentPositionFirst is > 0 or < 0 &&
                            x.CurrentPositionSecond is > 0 or < 0)
                    .Select(x => $"{x.TickerFirst},{x.TickerSecond}").Distinct().Count();

                // Размер позиции в процентах от портфеля
                double positionPercentPortfolio =
                    ((algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney / countUniqueTickersWithSignals) *
                     (percentSignals / 100.0)) / algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney * 100.0;

                // Размер позиции, руб
                double positionCost = algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney * positionPercentPortfolio / 100.0;

                // Цена инструмента
                (double First, double Second) lastPrice = await GetLastPriceAsync(tickerPair.Split(',')[0], tickerPair.Split(',')[1]);

                // Размер позиции, шт
                (double First, double Second) positionSize = GetPositionSize(tickerPair);
                
                // Применяем плечо
                var sharesTickers = (await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.AlgoShares)).Select(x => x.Ticker).ToList();
                var futuresTickers = (await tickerListUtilService.GetFuturesByTickerListAsync(KnownTickerLists.AlgoFutures)).Select(x => x.Ticker).ToList();

                if (sharesTickers.Contains(tickerPair.Split(',')[0]))
                {
                    positionSize.First *= algoConfigResource.MoneyManagementResource.ShareLeverage;
                    positionCost = 0.5 * positionCost + 0.5 * positionCost * algoConfigResource.MoneyManagementResource.ShareLeverage;
                }
                
                if (futuresTickers.Contains(tickerPair.Split(',')[0]))
                {
                    positionSize.First *= algoConfigResource.MoneyManagementResource.FutureLeverage;
                    positionCost = 0.5 * positionCost + 0.5 * positionCost * algoConfigResource.MoneyManagementResource.FutureLeverage;
                }

                if (sharesTickers.Contains(tickerPair.Split(',')[1]))
                {
                    positionSize.Second *= algoConfigResource.MoneyManagementResource.ShareLeverage;
                    positionCost = 0.5 * positionCost + 0.5 * positionCost * algoConfigResource.MoneyManagementResource.ShareLeverage;
                }
                
                if (futuresTickers.Contains(tickerPair.Split(',')[1]))
                {
                    positionSize.Second *= algoConfigResource.MoneyManagementResource.FutureLeverage;
                    positionCost = 0.5 * positionCost + 0.5 * positionCost * algoConfigResource.MoneyManagementResource.FutureLeverage;
                }

                await strategySignalRepository.UpdatePositionAsync(
                    new StatisticalArbitrageStrategySignal
                    {
                        TickerFirst = tickerPair.Split(',')[0],
                        TickerSecond = tickerPair.Split(',')[1],
                        CountStrategies = Math.Abs(countStrategies),
                        CountSignals = Math.Abs(countSignals),
                        PercentSignals = Math.Abs(percentSignals),
                        LastPriceFirst = lastPrice.First,
                        LastPriceSecond = lastPrice.Second,
                        PositionCost = positionCost,
                        PositionSizeFirst = Convert.ToInt32(positionSize.First),
                        PositionSizeSecond = Convert.ToInt32(positionSize.Second),
                        PositionPercentPortfolio = positionPercentPortfolio
                    });
            }

            catch (Exception exception)
            {
                logger.Error($"Ошибка CalculateStrategySignalsAsync '{tickerPair}', '{exception.Message}'");
            }
        }

        return true;

        async Task<(double First, double Second)> GetLastPriceAsync(string tickerFirst, string tickerSecond)
        {
            var sharesTickers = (await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.AlgoShares)).Select(x => x.Ticker).ToList();
            var futuresTickers = (await tickerListUtilService.GetFuturesByTickerListAsync(KnownTickerLists.AlgoFutures)).Select(x => x.Ticker).ToList();

            double lastPriceFirst = 0.0;
            double lastPriceSecond = 0.0;
            
            if (sharesTickers.Contains(tickerFirst)) lastPriceFirst = (await shareRepository.GetAsync(tickerFirst))!.LastPrice;
            if (futuresTickers.Contains(tickerFirst)) lastPriceFirst = (await futureRepository.GetAsync(tickerFirst))!.LastPrice;

            if (sharesTickers.Contains(tickerSecond)) lastPriceSecond = (await shareRepository.GetAsync(tickerSecond))!.LastPrice;
            if (futuresTickers.Contains(tickerSecond)) lastPriceSecond = (await futureRepository.GetAsync(tickerSecond))!.LastPrice;

            return (lastPriceFirst, lastPriceSecond);
        }

        (double First, double Second) GetPositionSize(string tickerPair)
        {
            double positionSizeFirst = 0.0;
            double positionSizeSecond = 0.0;
            
            foreach (var backtestResult in backtestResults.Where(x => $"{x.TickerFirst},{x.TickerSecond}" == tickerPair))
            {
                positionSizeFirst += backtestResult.CurrentPositionFirst;
                positionSizeSecond += backtestResult.CurrentPositionSecond;
            }

            return (positionSizeFirst, positionSizeSecond);
        }
        
        int GetCountSignals(string tickerPair)
        {
            int countSignals = 0;

            foreach (var backtestResult in backtestResults.Where(x => $"{x.TickerFirst},{x.TickerSecond}" == tickerPair))
            {
                var enable = statisticalArbitrageStrategyResources.Find(x => x.Id == backtestResult.StrategyId)!.Enable;

                if (!enable)
                    continue;

                if (backtestResult.CurrentPositionFirst != 0.0 && backtestResult.CurrentPositionSecond != 0.0)
                    countSignals++;
            }

            return countSignals;
        }        
    }

    /// <inheritdoc />
    public async Task<bool> OptimizeAsync()
    {
        var sw = Stopwatch.StartNew();
        
        Spreads = new ConcurrentDictionary<string, RegressionTail>(await algoHelper.GetSpreadsAsync());
        Candles = new ConcurrentDictionary<string, List<Candle>>(await algoHelper.GetStatisticalArbitrageCandlesAsync(true));
        Strategies = new ConcurrentDictionary<Guid, StatisticalArbitrageStrategy>(await algoHelper.GetStatisticalArbitrageStrategies());
        
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var statisticalArbitrageStrategyResources = await resourceStoreService.GetStatisticalArbitrageStrategiesAsync();

        await optimizationResultRepository.InvertDeleteAsync(statisticalArbitrageStrategyResources.Select(x => x.Id).ToList());

        int count = 0;

        foreach (var (strategyId, strategy) in Strategies)
        {
            await optimizationResultRepository.DeleteAsync(strategyId);

            var statisticalArbitrageStrategyResource = statisticalArbitrageStrategyResources.Find(x => x.Id == strategyId);

            if (statisticalArbitrageStrategyResource is null)
                continue;

            var optimizationResults = new List<StatisticalArbitrageOptimizationResult>();

            var tickerPairs = (await regressionTailRepository.GetAllAsync()).Select(x => $"{x.Ticker1},{x.Ticker2}");

            foreach (var tickerPair in tickerPairs)
            {
                strategy.Ticker = (tickerPair.Split(',')[0], tickerPair.Split(',')[1]);

                var syncCandles = algoHelper.SyncCandles(
                    Candles.TryGetValue(strategy.Ticker.First, out var candles1) ? candles1 : [], 
                    Candles.TryGetValue(strategy.Ticker.First, out var candles2) ? candles2 : []);
                    
                strategy.Candles = (syncCandles.Candles1, syncCandles.Candles2);

                if (strategy.Candles.First is [] || strategy.Candles.Second is [])
                    continue;
                
                var tail = await regressionTailRepository.GetAsync(strategy.Ticker.First, strategy.Ticker.Second);
                    
                if (tail is null)
                    continue;

                strategy.Spreads = tail.Tails;

                var parameterSets = AlgoHelper.GetParameterSets(statisticalArbitrageStrategyResource.Params);

                foreach (var parameterSet in parameterSets)
                {
                    try
                    {
                        if (parameterSet.Count == 0)
                            continue;

                        strategy.InitForParameterSet(
                            parameterSet, 
                            algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1, 
                            algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney, 
                            algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney); 

                        await strategy.Execute();
                        
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
    
    /// <inheritdoc />
    public async Task CalculateCorrelationAsync()
    {
        var shares =
            await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.StatisticalArbitrageShares);
        var futures =
            (await tickerListUtilService.GetFuturesByTickerListAsync(KnownTickerLists.StatisticalArbitrageFutures))
            .Where(x => x.ExpirationDate > DateOnly.FromDateTime(DateTime.Today)).ToList();

        var candles = new Dictionary<string, List<DailyCandle>>();

        var from = DateOnly.FromDateTime(DateTime.Today.AddYears(-1));
        var to = DateOnly.FromDateTime(DateTime.Today);

        foreach (var share in shares)
            candles.TryAdd(share.Ticker, await dailyCandleRepository.GetAsync(share.Ticker, from, to));

        foreach (var future in futures)
            candles.TryAdd(future.Ticker, await dailyCandleRepository.GetAsync(future.Ticker, from, to));

        List<string> tickers =
        [
            ..shares.Select(x => x.Ticker),
            ..futures.Select(x => x.Ticker)
        ];

        // Очистим таблицу
        await correlationRepository.DeleteAsync();

        for (int i = 0; i < tickers.Count; i++)
        {
            for (int j = i + 1; j < tickers.Count; j++)
            {
                try
                {
                    // Получаем свечи и синхронизируем массивы по дате
                    var syncCandles = algoHelper.SyncCandles(candles[tickers[i]], candles[tickers[j]]);

                    var prices1 = syncCandles.Candles1.Select(x => x.Close).ToList();
                    var prices2 = syncCandles.Candles2.Select(x => x.Close).ToList();

                    // Логарифмируем
                    var logValues1 = prices1.Log();
                    var logValues2 = prices2.Log();

                    // Центрируем
                    var centeringValues1 = logValues1.Centering();
                    var centeringValues2 = logValues2.Centering();

                    // Делим на стандартное отклонение
                    var divStdValues1 = centeringValues1.DivConst(centeringValues1.StdDev());
                    var divStdValues2 = centeringValues2.DivConst(centeringValues2.StdDev());

                    // Приращения
                    var incrementValues1 = divStdValues1.Increments();
                    var incrementValues2 = divStdValues2.Increments();

                    // Расчет корреляции
                    double correlation = incrementValues1.Correlation(incrementValues2);

                    if (Math.Abs(correlation - 1.0) < 0.0001)
                        continue;

                    if (Math.Abs(correlation + 1.0) < 0.0001)
                        continue;

                    if (correlation == 0.0)
                        continue;

                    if (Math.Abs(correlation) < 0.8)
                        continue;

                    var correlationModel = new Correlation
                    {
                        Ticker1 = tickers[i],
                        Ticker2 = tickers[j],
                        Value = correlation
                    };

                    await correlationRepository.AddAsync(correlationModel);
                }

                catch (Exception exception)
                {
                    logger.Error(exception, "Ошибка расчета корреляции. {ticker1}, {ticker2}", tickers[i], tickers[j]);
                }
            }
        }
    }

    /// <inheritdoc />
    public Task<Dictionary<string, RegressionTail>> CalculateRegressionTailsAsync() => 
        algoHelper.CalculateRegressionTailsAsync(DateOnly.FromDateTime(DateTime.Today.AddYears(-1)), DateOnly.FromDateTime(DateTime.Today));
}