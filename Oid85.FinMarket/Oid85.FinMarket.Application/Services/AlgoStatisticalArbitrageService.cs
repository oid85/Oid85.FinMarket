using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Accord.Statistics.Models.Regression.Linear;
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
    IComputationService computationService,
    ITickerListUtilService tickerListUtilService,
    AlgoHelper algoHelper)
    : IAlgoStatisticalArbitrageService
{
    public ConcurrentDictionary<string, List<Candle>> DailyCandles { get; set; } = new();

    public ConcurrentDictionary<string, RegressionTail> Spreads { get; set; } = new();
    public ConcurrentDictionary<Guid, StatisticalArbitrageStrategy> StrategyDictionary { get; set; } = new();

    public async Task<bool> BacktestAsync()
    {
        await InitBacktestAsync();

        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var statisticalArbitrageStrategyResources = await resourceStoreService.GetStatisticalArbitrageStrategiesAsync();

        var optimizationResults = await optimizationResultRepository.GetAsync(algoConfigResource.OptimizationResultFilterResource);

        await backtestResultRepository.InvertDeleteAsync(statisticalArbitrageStrategyResources.Select(x => x.Id).ToList());

        foreach (var (strategyId, strategy) in StrategyDictionary)
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
                    strategy.StabilizationPeriod = algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1;
                    strategy.StartMoney = algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney;
                    strategy.EndMoney = algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney;
                    strategy.Ticker = (optimizationResult.TickerFirst, optimizationResult.TickerSecond);

                    var syncCandles = SyncCandles(
                        DailyCandles.TryGetValue(strategy.Ticker.First, out var candles1) ? candles1 : [], 
                        DailyCandles.TryGetValue(strategy.Ticker.First, out var candles2) ? candles2 : []);
                    
                    strategy.Candles = (syncCandles.Candles1, syncCandles.Candles2);

                    if (strategy.Candles.First is [])
                        continue;

                    if (strategy.Candles.Second is [])
                        continue;
                    
                    if (strategy.Candles.First.Count < strategy.StabilizationPeriod + 1)
                        continue;

                    if (strategy.Candles.Second.Count < strategy.StabilizationPeriod + 1)
                        continue;

                    var tail = await regressionTailRepository.GetAsync(strategy.Ticker.First, strategy.Ticker.Second);
                    
                    if (tail is null)
                        continue;

                    strategy.Spreads = tail.Tails;
                    
                    var parameterSet = JsonSerializer.Deserialize<Dictionary<string, int>>(optimizationResult.StrategyParams);

                    if (parameterSet is null)
                        continue;

                    strategy.Parameters = parameterSet;
                    strategy.Positions.Clear();
                    strategy.EqiutyCurve.Clear();
                    strategy.DrawdownCurve.Clear();
                    strategy.EndMoney = algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney;

                    strategy.GraphPoints.Clear();
                    for (int i = 0; i < strategy.Candles.First.Count; i++)
                        strategy.GraphPoints.Add(new ArbitrageGraphPoint());

                    strategy.Execute();

                    var backtestResult = AlgoMapper.MapToBacktestResult(strategy);
                    backtestResults.Add(backtestResult);
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

    public async Task<(StatisticalArbitrageBacktestResult? backtestResult, StatisticalArbitrageStrategy? strategy)> BacktestAsync(Guid backtestResultId)
    {
        try
        {
            var backtestResult = await backtestResultRepository.GetAsync(backtestResultId);

            if (backtestResult is null)
                return (null, null);

            await InitBacktestAsync(backtestResult.TickerFirst, backtestResult.TickerSecond, backtestResult.StrategyId);

            var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
            var statisticalArbitrageStrategyResources = await resourceStoreService.GetStatisticalArbitrageStrategiesAsync();

            var statisticalArbitrageStrategyResource = statisticalArbitrageStrategyResources.Find(x => x.Id == backtestResult.StrategyId);

            if (statisticalArbitrageStrategyResource is null)
                return (null, null);

            var strategy = StrategyDictionary[backtestResult.StrategyId];

            strategy.StabilizationPeriod = algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1;
            strategy.StartMoney = algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney;
            strategy.EndMoney = algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney;
            strategy.Ticker = (backtestResult.TickerFirst, backtestResult.TickerSecond);
            
            var syncCandles = SyncCandles(
                DailyCandles.TryGetValue(strategy.Ticker.First, out var candles1) ? candles1 : [], 
                DailyCandles.TryGetValue(strategy.Ticker.First, out var candles2) ? candles2 : []);
                    
            strategy.Candles = (syncCandles.Candles1, syncCandles.Candles2);

            if (strategy.Candles.First is [])
                return (null, null);

            if (strategy.Candles.Second is [])
                return (null, null);
                    
            if (strategy.Candles.First.Count < strategy.StabilizationPeriod + 1)
                return (null, null);

            if (strategy.Candles.Second.Count < strategy.StabilizationPeriod + 1)
                return (null, null);

            var tail = await regressionTailRepository.GetAsync(strategy.Ticker.First, strategy.Ticker.Second);
                    
            if (tail is null)
                return (null, null);

            strategy.Spreads = tail.Tails;
            
            var parameterSet = JsonSerializer.Deserialize<Dictionary<string, int>>(backtestResult.StrategyParams);

            if (parameterSet is null)
                return (null, null);

            strategy.Parameters = parameterSet;
            strategy.Positions.Clear();
            strategy.EqiutyCurve.Clear();
            strategy.DrawdownCurve.Clear();
            strategy.EndMoney = algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney;

            strategy.GraphPoints.Clear();
            for (int i = 0; i < strategy.Candles.First.Count; i++)
                strategy.GraphPoints.Add(new ArbitrageGraphPoint());

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
            var sharesTickers = (await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.AlgoShares))
                .Select(x => x.Ticker).ToList();
            var futuresTickers = (await tickerListUtilService.GetFuturesByTickerListAsync(KnownTickerLists.AlgoFutures))
                .Select(x => x.Ticker).ToList();

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

    public async Task<bool> OptimizeAsync()
    {
        var sw = Stopwatch.StartNew();

        await InitOptimizationAsync();

        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var statisticalArbitrageStrategyResources = await resourceStoreService.GetStatisticalArbitrageStrategiesAsync();

        await optimizationResultRepository.InvertDeleteAsync(statisticalArbitrageStrategyResources.Select(x => x.Id).ToList());

        int count = 0;

        foreach (var (strategyId, strategy) in StrategyDictionary)
        {
            await optimizationResultRepository.DeleteAsync(strategyId);

            var statisticalArbitrageStrategyResource = statisticalArbitrageStrategyResources.Find(x => x.Id == strategyId);

            if (statisticalArbitrageStrategyResource is null)
                continue;

            var optimizationResults = new List<StatisticalArbitrageOptimizationResult>();

            var tickerPairs = (await regressionTailRepository.GetAllAsync()).Select(x => $"{x.Ticker1},{x.Ticker2}");

            foreach (var tickerPair in tickerPairs)
            {
                strategy.StabilizationPeriod = algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1;
                strategy.StartMoney = algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney;
                strategy.EndMoney = algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney;
                strategy.Ticker = (tickerPair.Split(',')[0], tickerPair.Split(',')[1]);

                var syncCandles = SyncCandles(
                    DailyCandles.TryGetValue(strategy.Ticker.First, out var candles1) ? candles1 : [], 
                    DailyCandles.TryGetValue(strategy.Ticker.First, out var candles2) ? candles2 : []);
                    
                strategy.Candles = (syncCandles.Candles1, syncCandles.Candles2);

                if (strategy.Candles.First is [])
                    continue;

                if (strategy.Candles.Second is [])
                    continue;
                    
                if (strategy.Candles.First.Count < strategy.StabilizationPeriod + 1)
                    continue;

                if (strategy.Candles.Second.Count < strategy.StabilizationPeriod + 1)
                    continue;
                
                var tail = await regressionTailRepository.GetAsync(strategy.Ticker.First, strategy.Ticker.Second);
                    
                if (tail is null)
                    continue;

                strategy.Spreads = tail.Tails;

                var parameterSets = algoHelper.GetParameterSets(statisticalArbitrageStrategyResource.Params);

                foreach (var parameterSet in parameterSets)
                {
                    try
                    {
                        if (parameterSet.Count == 0)
                            continue;

                        strategy.Parameters = parameterSet;
                        strategy.Positions.Clear();
                        strategy.EqiutyCurve.Clear();
                        strategy.DrawdownCurve.Clear();
                        strategy.EndMoney = algoConfigResource.MoneyManagementResource.StatisticalArbitrageMoney;

                        strategy.GraphPoints.Clear();
                        for (int i = 0; i < strategy.Candles.First.Count; i++)
                            strategy.GraphPoints.Add(new ArbitrageGraphPoint());

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
    
    private async Task InitBacktestAsync(string? ticker1 = null, string? ticker2 = null, Guid? strategyId = null)
    {
        await InitDailyCandlesAsync(false, ticker1, ticker2);
        await InitSpreadsAsync(ticker1, ticker2);
        
        StrategyDictionary = new ConcurrentDictionary<Guid, StatisticalArbitrageStrategy>(await algoHelper.GetStatisticalArbitrageStrategies(strategyId));
    }

    private async Task InitOptimizationAsync()
    {
        await InitDailyCandlesAsync(true);
        await InitSpreadsAsync();

        StrategyDictionary = new ConcurrentDictionary<Guid, StatisticalArbitrageStrategy>(await algoHelper.GetStatisticalArbitrageStrategies());
    }

    private async Task InitDailyCandlesAsync(bool isOptimization, string? ticker1 = null, string? ticker2 = null)
    {
        var dates = await GetDatesAsync(isOptimization);

        var tickers = ticker1 is null || ticker2 is null 
            ? await algoHelper.GetAllTickersForStatisticalArbitrageAsync() 
            : [ticker1, ticker2];

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

    private async Task InitSpreadsAsync(string? ticker1 = null, string? ticker2 = null)
    {
        var tails = ticker1 is null || ticker2 is null 
            ? await regressionTailRepository.GetAllAsync()
            : (await regressionTailRepository.GetAllAsync()).Where(x => x.Ticker1 == ticker1 && x.Ticker2 == ticker2);

        foreach (var tail in tails)
        {
            if (tail.Tails.Count == 0)
                continue;

            Spreads.TryAdd($"{tail.Ticker1},{tail.Ticker2}", tail);
        }
    }    
    
    private async Task<(DateOnly From, DateOnly To)> GetDatesAsync(bool isOptimization) => 
        isOptimization ? await algoHelper.GetOptimizationDatesAsync() : await algoHelper.GetBacktestDatesAsync();

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
                    var syncCandles = SyncCandles(candles[tickers[i]], candles[tickers[j]]);

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
        CalculateRegressionTailsAsync(DateOnly.FromDateTime(DateTime.Today.AddYears(-5)), DateOnly.FromDateTime(DateTime.Today));

    private async Task<Dictionary<string, RegressionTail>> CalculateRegressionTailsAsync(DateOnly from, DateOnly to)
    {
        // Очистим таблицу
        await regressionTailRepository.DeleteAsync();

        var correlations = (await correlationRepository.GetAllAsync()).ToList();

        var tails = new Dictionary<string, RegressionTail>();

        foreach (var correlation in correlations)
        {
            try
            {
                // Получаем и синхронизируем свечи
                var candles1 = await dailyCandleRepository.GetAsync(correlation.Ticker1, from, to);
                var candles2 = await dailyCandleRepository.GetAsync(correlation.Ticker2, from, to);

                var syncCandles = SyncCandles(candles1, candles2);

                // Declare some sample test data.
                double[] inputs = syncCandles.Candles2.Select(x => x.Close).ToArray();
                double[] outputs = syncCandles.Candles1.Select(x => x.Close).ToArray();

                // Use Ordinary Least Squares to learn the regression
                var ols = new OrdinaryLeastSquares();

                // Use OLS to learn the simple linear regression
                SimpleLinearRegression regression = ols.Learn(inputs, outputs);

                // We can also extract the slope and the intercept term for the line
                double slope = regression.Slope;
                double intercept = regression.Intercept;

                string key = $"{correlation.Ticker1},{correlation.Ticker2}";

                // Расчет хвостов
                var regressionTails = new List<RegressionTailItem>();

                for (int i = 0; i < syncCandles.Candles1.Count; i++)
                {
                    double y = slope * syncCandles.Candles2[i].Close + intercept;
                    double tailValue = syncCandles.Candles1[i].Close - y;

                    if (!tails.ContainsKey(key))
                        tails.Add(key,
                            new RegressionTail { Ticker1 = correlation.Ticker1, Ticker2 = correlation.Ticker2 });

                    regressionTails.Add(new RegressionTailItem
                        { Date = syncCandles.Candles1[i].Date, Value = tailValue });
                }

                tails[key].Slope = slope;
                tails[key].Intercept = intercept;

                // Расчитаем Z-score
                tails[key].Tails = ZScore(regressionTails);

                // Проверяем на стационарность и сохраняем
                var isStationary = await computationService.CheckStationaryAsync(
                    [tails[key].Tails.Select(x => x.Value).ToList()]);

                if (isStationary[0])
                    await regressionTailRepository.AddAsync(tails[key]);
                else
                    tails.Remove(key);
            }

            catch (Exception exception)
            {
                logger.Error(exception, "Ошибка расчета остатков регрессии. {ticker1}, {ticker2}", correlation.Ticker1,
                    correlation.Ticker2);
            }
        }

        return tails;
    }

    private static (List<DailyCandle> Candles1, List<DailyCandle> Candles2) SyncCandles(List<DailyCandle> candles1,
        List<DailyCandle> candles2)
    {
        var dates1 = candles1.Select(x => x.Date).ToList();
        var dates2 = candles2.Select(x => x.Date).ToList();

        var dates = dates1.Intersect(dates2).ToList();

        var resultCandles1 = candles1.Where(x => dates.Contains(x.Date)).OrderBy(x => x.Date).ToList();
        var resultCandles2 = candles2.Where(x => dates.Contains(x.Date)).OrderBy(x => x.Date).ToList();

        return (resultCandles1, resultCandles2);
    }

    private static (List<Candle> Candles1, List<Candle> Candles2) SyncCandles(List<Candle> candles1,
        List<Candle> candles2)
    {
        var dates1 = candles1.Select(x => x.DateTime.Date).ToList();
        var dates2 = candles2.Select(x => x.DateTime.Date).ToList();

        var dates = dates1.Intersect(dates2).ToList();

        var resultCandles1 = candles1.Where(x => dates.Contains(x.DateTime.Date)).OrderBy(x => x.DateTime.Date).ToList();
        var resultCandles2 = candles2.Where(x => dates.Contains(x.DateTime.Date)).OrderBy(x => x.DateTime.Date).ToList();

        return (resultCandles1, resultCandles2);
    }    
    
    /// <summary>
    /// Z-score
    /// </summary>
    public static List<RegressionTailItem> ZScore(List<RegressionTailItem> values)
    {
        if (values.Count == 0)
            return [];

        var dates = values.Select(x => x.Date).ToList();
        var tailValues = values.Select(x => x.Value).ToList();

        var average = tailValues.Average();
        var stdDev = tailValues.StdDev();

        if (stdDev == 0.0)
            return [];

        var zScoreValues = tailValues.AddConst(-1 * average).DivConst(stdDev);

        var result = new List<RegressionTailItem>();

        for (int i = 0; i < dates.Count; i++)
            result.Add(new RegressionTailItem { Date = dates[i], Value = zScoreValues[i] });

        return result;
    }
}