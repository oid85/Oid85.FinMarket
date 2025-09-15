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
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services;

public class PairArbitrageService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IResourceStoreService resourceStoreService,
    IPairArbitrageBacktestResultRepository backtestResultRepository,
    IPairArbitrageOptimizationResultRepository optimizationResultRepository,
    IPairArbitrageStrategySignalRepository strategySignalRepository,
    IShareRepository shareRepository,
    IFutureRepository futureRepository,
    ICorrelationRepository correlationRepository,
    IRegressionTailRepository regressionTailRepository,
    ITickerListUtilService tickerListUtilService,
    AlgoHelper algoHelper)
    : IPairArbitrageService
{
    private ConcurrentDictionary<string, List<Candle>> Candles { get; set; } = new();
    private ConcurrentDictionary<string, RegressionTail> Spreads { get; set; } = new();
    private ConcurrentDictionary<Guid, PairArbitrageStrategy> Strategies { get; set; } = new();

    /// <inheritdoc />
    public async Task<bool> BacktestAsync()
    {
        Spreads = new ConcurrentDictionary<string, RegressionTail>(await algoHelper.GetSpreadsAsync());
        Candles = new ConcurrentDictionary<string, List<Candle>>(await algoHelper.GetPairArbitrageCandlesAsync(false));
        Strategies = new ConcurrentDictionary<Guid, PairArbitrageStrategy>(await algoHelper.GetPairArbitrageStrategies());
        
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var pairArbitrageStrategyResources = await resourceStoreService.GetPairArbitrageStrategiesAsync();

        var optimizationResults = await optimizationResultRepository.GetAsync(algoConfigResource.PairArbitrageOptimizationResultFilterResource);

        await backtestResultRepository.InvertDeleteAsync(pairArbitrageStrategyResources.Select(x => x.Id).ToList());

        foreach (var (strategyId, strategy) in Strategies)
        {
            await backtestResultRepository.DeleteAsync(strategyId);

            var pairArbitrageStrategyResource = pairArbitrageStrategyResources.Find(x => x.Id == strategyId);

            if (pairArbitrageStrategyResource is null)
                continue;

            var backtestResults = new List<PairArbitrageBacktestResult>();

            foreach (var optimizationResult in optimizationResults.Where(x => x.StrategyId == strategyId))
            {
                try
                {
                    strategy.Ticker = (optimizationResult.TickerFirst, optimizationResult.TickerSecond);
                    strategy.IsFuture = (await algoHelper.IsFuture(optimizationResult.TickerFirst), await algoHelper.IsFuture(optimizationResult.TickerSecond));
                    strategy.BasicAssetSize = (await algoHelper.GetBasicAssetSize(optimizationResult.TickerFirst), await algoHelper.GetBasicAssetSize(optimizationResult.TickerSecond));
                    strategy.Leverage = await algoHelper.GetPairArbitrageLeverage(optimizationResult.TickerFirst, optimizationResult.TickerSecond);
                    
                    var syncCandles = AlgoHelper.SyncCandles(
                        Candles.TryGetValue(strategy.Ticker.First, out var firstCandles) ? firstCandles : [], 
                        Candles.TryGetValue(strategy.Ticker.Second, out var secondCandles) ? secondCandles : []);
                    
                    strategy.Candles = (syncCandles.First, syncCandles.Second);

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
                        algoConfigResource.MoneyManagementResource.PairArbitrageMoney, 
                        algoConfigResource.MoneyManagementResource.PairArbitrageMoney); 

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
    public async Task<(PairArbitrageBacktestResult? backtestResult, PairArbitrageStrategy? strategy)> BacktestAsync(Guid backtestResultId)
    {
        try
        {
            var backtestResult = await backtestResultRepository.GetAsync(backtestResultId);

            if (backtestResult is null)
                return (null, null);
            
            Spreads = new ConcurrentDictionary<string, RegressionTail>(await algoHelper.GetSpreadsAsync(backtestResult.TickerFirst, backtestResult.TickerSecond));
            Candles = new ConcurrentDictionary<string, List<Candle>>(await algoHelper.GetPairArbitrageCandlesAsync(false, backtestResult.TickerFirst, backtestResult.TickerSecond));
            Strategies = new ConcurrentDictionary<Guid, PairArbitrageStrategy>(await algoHelper.GetPairArbitrageStrategies(backtestResult.StrategyId));
            
            var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
            var pairArbitrageStrategyResources = await resourceStoreService.GetPairArbitrageStrategiesAsync();

            var pairArbitrageStrategyResource = pairArbitrageStrategyResources.Find(x => x.Id == backtestResult.StrategyId);

            if (pairArbitrageStrategyResource is null)
                return (null, null);

            var strategy = Strategies[backtestResult.StrategyId];
            
            strategy.Ticker = (backtestResult.TickerFirst, backtestResult.TickerSecond);
            strategy.IsFuture = (await algoHelper.IsFuture(backtestResult.TickerFirst), await algoHelper.IsFuture(backtestResult.TickerSecond));
            strategy.BasicAssetSize = (await algoHelper.GetBasicAssetSize(backtestResult.TickerFirst), await algoHelper.GetBasicAssetSize(backtestResult.TickerSecond));
            strategy.Leverage = await algoHelper.GetPairArbitrageLeverage(backtestResult.TickerFirst, backtestResult.TickerSecond);
            
            var syncCandles = AlgoHelper.SyncCandles(
                Candles.TryGetValue(strategy.Ticker.First, out var firstCandles) ? firstCandles : [], 
                Candles.TryGetValue(strategy.Ticker.Second, out var secondCandles) ? secondCandles : []);
                    
            strategy.Candles = (syncCandles.First, syncCandles.Second);

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
                algoConfigResource.MoneyManagementResource.PairArbitrageMoney, 
                algoConfigResource.MoneyManagementResource.PairArbitrageMoney); 

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
        var pairArbitrageStrategyResources = await resourceStoreService.GetPairArbitrageStrategiesAsync();

        var backtestResults = await backtestResultRepository.GetAsync(algoConfigResource.PairArbitrageBacktestResultFilterResource);

        // Добавляем тикеры, если их еще нет в таблице
        var tickersInStrategySignals = (await strategySignalRepository.GetAllAsync()).Select(x => $"{x.TickerFirst},{x.TickerSecond}").ToList();
        var tickersInBacktestResults = backtestResults.Select(x => $"{x.TickerFirst},{x.TickerSecond}").Distinct().ToList();
        
        foreach (var tickerPair in tickersInStrategySignals)
            if (!tickersInBacktestResults.Contains(tickerPair))
                await strategySignalRepository.UpdatePositionAsync(
                    new PairArbitrageStrategySignal(tickerPair.Split(',')[0], tickerPair.Split(',')[1]));

        // Расчет для каждого тикера
        foreach (var tickerPair in tickersInBacktestResults)
        {
            try
            {
                string tickerFirst = tickerPair.Split(',')[0];
                string tickerSecond = tickerPair.Split(',')[1];
                
                if (!tickersInStrategySignals.Contains(tickerPair))
                    await strategySignalRepository.AddAsync(
                        new PairArbitrageStrategySignal(tickerFirst, tickerSecond));

                var backtestResultsByTickerPair = backtestResults.Where(x => $"{x.TickerFirst},{x.TickerSecond}" == tickerPair).ToList();
                
                // Количество сигналов
                int countSignals = GetCountSignals(tickerPair);

                // Количество стратегий
                int countStrategies = backtestResultsByTickerPair.Count;

                // Процент сигналов
                double percentSignals = Convert.ToDouble(countSignals) / Convert.ToDouble(countStrategies) * 100.0;

                // Количество уникальных пар тикеров
                int countUniqueTickers = backtestResults.Where(x => x.NumberPositions > 0)
                    .Select(x => $"{x.TickerFirst},{x.TickerSecond}").Distinct().Count();

                // Размер позиции в процентах от портфеля
                double positionPercentPortfolio = 0.0;
                
                if (countUniqueTickers != 0 || percentSignals != 0.0)
                    positionPercentPortfolio = (100.0 / countUniqueTickers) * (percentSignals / 100.0);

                // Размер позиции, руб
                double positionCost = algoConfigResource.MoneyManagementResource.PairArbitrageMoney * positionPercentPortfolio / 100.0;

                // Цена инструмента
                (double First, double Second) lastPrice = await GetLastPriceAsync(tickerFirst, tickerSecond);

                // Размер позиции, шт
                (double First, double Second) positionSize = GetPositionSize(positionCost, lastPrice);

                // Определяем знак позиции
                if (backtestResultsByTickerPair.Sum(x => x.CurrentPositionFirst) < 0) positionSize.First *= -1.0;
                if (backtestResultsByTickerPair.Sum(x => x.CurrentPositionSecond) < 0) positionSize.Second *= -1.0;

                // Применяем плечо
                var sharesTickers = (await shareRepository.GetAllAsync()).Select(x => x.Ticker).ToList();
                var futuresTickers = (await futureRepository.GetAllAsync()).Select(x => x.Ticker).ToList();

                if (sharesTickers.Contains(tickerFirst))
                {
                    positionSize.First *= algoConfigResource.MoneyManagementResource.ShareLeverage;
                    positionCost = (positionCost / 2.0) + (positionCost / 2.0) * algoConfigResource.MoneyManagementResource.ShareLeverage;
                }
                
                if (futuresTickers.Contains(tickerFirst))
                {
                    positionSize.First *= algoConfigResource.MoneyManagementResource.FutureLeverage;
                    positionCost = (positionCost / 2.0) + (positionCost / 2.0) * algoConfigResource.MoneyManagementResource.FutureLeverage;
                }

                if (sharesTickers.Contains(tickerSecond))
                {
                    positionSize.Second *= algoConfigResource.MoneyManagementResource.ShareLeverage;
                    positionCost = (positionCost / 2.0) + (positionCost / 2.0) * algoConfigResource.MoneyManagementResource.ShareLeverage;
                }
                
                if (futuresTickers.Contains(tickerSecond))
                {
                    positionSize.Second *= algoConfigResource.MoneyManagementResource.FutureLeverage;
                    positionCost = (positionCost / 2.0) + (positionCost / 2.0) * algoConfigResource.MoneyManagementResource.FutureLeverage;
                }

                await strategySignalRepository.UpdatePositionAsync(
                    new PairArbitrageStrategySignal
                    {
                        TickerFirst = tickerFirst,
                        TickerSecond = tickerSecond,
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
            var sharesTickers = (await shareRepository.GetAllAsync()).Select(x => x.Ticker).ToList();
            var futuresTickers = (await futureRepository.GetAllAsync()).Select(x => x.Ticker).ToList();

            double lastPriceFirst = 0.0;
            double lastPriceSecond = 0.0;
            
            if (sharesTickers.Contains(tickerFirst)) lastPriceFirst = (await shareRepository.GetAsync(tickerFirst))!.LastPrice;
            if (futuresTickers.Contains(tickerFirst)) lastPriceFirst = (await futureRepository.GetAsync(tickerFirst))!.LastPrice;

            if (sharesTickers.Contains(tickerSecond)) lastPriceSecond = (await shareRepository.GetAsync(tickerSecond))!.LastPrice;
            if (futuresTickers.Contains(tickerSecond)) lastPriceSecond = (await futureRepository.GetAsync(tickerSecond))!.LastPrice;

            return (lastPriceFirst, lastPriceSecond);
        }

        (double First, double Second) GetPositionSize(double positionCost, (double First, double Second) lastPrice)
        {
            double money = positionCost / 2.0;

            double positionSizeFirst = money / lastPrice.First;
            double positionSizeSecond = money / lastPrice.Second;

            return (positionSizeFirst, positionSizeSecond);
        }
        
        int GetCountSignals(string tickerPair)
        {
            int countSignals = 0;

            foreach (var backtestResult in backtestResults.Where(x => $"{x.TickerFirst},{x.TickerSecond}" == tickerPair))
            {
                var enable = pairArbitrageStrategyResources.Find(x => x.Id == backtestResult.StrategyId)!.Enable;

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
        Candles = new ConcurrentDictionary<string, List<Candle>>(await algoHelper.GetPairArbitrageCandlesAsync(true));
        Strategies = new ConcurrentDictionary<Guid, PairArbitrageStrategy>(await algoHelper.GetPairArbitrageStrategies());
        
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        var pairArbitrageStrategyResources = await resourceStoreService.GetPairArbitrageStrategiesAsync();

        await optimizationResultRepository.InvertDeleteAsync(pairArbitrageStrategyResources.Select(x => x.Id).ToList());

        int count = 0;

        foreach (var (strategyId, strategy) in Strategies)
        {
            await optimizationResultRepository.DeleteAsync(strategyId);

            var pairArbitrageStrategyResource = pairArbitrageStrategyResources.Find(x => x.Id == strategyId);

            if (pairArbitrageStrategyResource is null)
                continue;

            var optimizationResults = new List<PairArbitrageOptimizationResult>();

            var tickerPairs = (await regressionTailRepository.GetAllAsync()).Select(x => $"{x.TickerFirst},{x.TickerSecond}");

            foreach (var tickerPair in tickerPairs)
            {
                string tickerFirst = tickerPair.Split(',')[0];
                string tickerSecond = tickerPair.Split(',')[1];
                
                strategy.Ticker = (tickerFirst, tickerSecond);
                strategy.IsFuture = (await algoHelper.IsFuture(tickerFirst), await algoHelper.IsFuture(tickerSecond));
                strategy.BasicAssetSize = (await algoHelper.GetBasicAssetSize(tickerFirst), await algoHelper.GetBasicAssetSize(tickerSecond));
                strategy.Leverage = await algoHelper.GetPairArbitrageLeverage(tickerFirst, tickerSecond);
                
                var syncCandles = AlgoHelper.SyncCandles(
                    Candles.TryGetValue(strategy.Ticker.First, out var firstCandles) ? firstCandles : [], 
                    Candles.TryGetValue(strategy.Ticker.Second, out var secondCandles) ? secondCandles : []);
                    
                strategy.Candles = (syncCandles.First, syncCandles.Second);

                if (strategy.Candles.First is [] || strategy.Candles.Second is [])
                    continue;
                
                var tail = await regressionTailRepository.GetAsync(strategy.Ticker.First, strategy.Ticker.Second);
                    
                if (tail is null)
                    continue;

                strategy.Spreads = tail.Tails;

                var parameterSets = AlgoHelper.GetParameterSets(pairArbitrageStrategyResource.Params);

                foreach (var parameterSet in parameterSets)
                {
                    try
                    {
                        if (parameterSet.Count == 0)
                            continue;

                        strategy.InitForParameterSet(
                            parameterSet, 
                            algoConfigResource.PeriodConfigResource.StabilizationPeriodInCandles + 1, 
                            algoConfigResource.MoneyManagementResource.PairArbitrageMoney, 
                            algoConfigResource.MoneyManagementResource.PairArbitrageMoney); 

                        await strategy.Execute();
                        
                        optimizationResults.Add(AlgoMapper.MapToOptimizationResult(strategy));
                    }

                    catch (Exception exception)
                    {
                        logger.Info($"Оптимизация '{strategy.Ticker.First},{strategy.Ticker.Second}', '{strategy.StrategyName}', '{JsonSerializer.Serialize(parameterSet)}'. {exception}");
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
        var shares = await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.PairArbitrageShares);
        var futures = (await tickerListUtilService.GetFuturesByTickerListAsync(KnownTickerLists.PairArbitrageFutures))
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
                    var syncCandles = AlgoHelper.SyncCandles(candles[tickers[i]], candles[tickers[j]]);

                    var firstPrices = syncCandles.First.Select(x => x.Close).ToList();
                    var secondPrices = syncCandles.Second.Select(x => x.Close).ToList();

                    // Логарифмируем
                    var firstLogValues = firstPrices.Log();
                    var secondLogValues = secondPrices.Log();

                    // Центрируем
                    var firstCenteringValues = firstLogValues.Centering();
                    var secondCenteringValues = secondLogValues.Centering();

                    // Делим на стандартное отклонение
                    var firstDivStdValues = firstCenteringValues.DivConst(firstCenteringValues.StdDev());
                    var secondDivStdValues = secondCenteringValues.DivConst(secondCenteringValues.StdDev());

                    // Приращения
                    var firstIncrementValues = firstDivStdValues.Increments();
                    var secondIncrementValues = secondDivStdValues.Increments();

                    // Расчет корреляции
                    double correlation = firstIncrementValues.Correlation(secondIncrementValues);

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
                        TickerFirst = tickers[i],
                        TickerSecond = tickers[j],
                        Value = correlation
                    };

                    await correlationRepository.AddAsync(correlationModel);
                }

                catch (Exception exception)
                {
                    logger.Error(exception, "Ошибка расчета корреляции. {tickerFirst}, {tickerSecond}", tickers[i], tickers[j]);
                }
            }
        }
    }

    /// <inheritdoc />
    public async Task<Dictionary<string, RegressionTail>> CalculateRegressionTailsAsync()
    {
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        return await algoHelper.CalculateRegressionTailsAsync(
            DateOnly.FromDateTime(DateTime.Today.AddDays(-1 * algoConfigResource.PeriodConfigResource.CalculateRegressionTailsPeriodInDays)), 
            DateOnly.FromDateTime(DateTime.Today));
    }
}