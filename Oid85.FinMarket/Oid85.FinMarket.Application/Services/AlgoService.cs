using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.Application.Services;

public class AlgoService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IHourlyCandleRepository hourlyCandleRepository,
    IResourceStoreService resourceStoreService,
    IBacktestResultRepository backtestResultRepository,
    IOptimizationResultRepository optimizationResultRepository,
    IStrategySignalRepository strategySignalRepository,
    IShareRepository shareRepository,
    IFutureRepository futureRepository,
    IServiceProvider serviceProvider,
    ITickerListUtilService tickerListUtilService)
    : IAlgoService
{
    public ConcurrentDictionary<string, List<Candle>> DailyCandles { get; set; } = new();
    
    public ConcurrentDictionary<string, List<Candle>> HourlyCandles { get; set; } = new();
    
    public ConcurrentDictionary<Guid, Strategy> StrategyDictionary { get; set; } = new();
    
    private AlgoConfigResource _algoConfigResource = new();
    
    private List<AlgoStrategyResource> _algoStrategyResources = new();
    
    private bool _isOptimization;
    
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

                    strategy.Execute();

                    var backtestResult = CreateBacktestResult(strategy);
                    backtestResults.Add(backtestResult);
                }
                
                catch (Exception exception)
                {
                    logger.Info($"Бэктест '{optimizationResult.Ticker}', '{optimizationResult.StrategyName}', '{optimizationResult.StrategyParams}'. {exception}");
                }  
            }
            
            await backtestResultRepository.AddAsync(backtestResults);
        }
        
        await CalculateStrategySignalsAsync();
        
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

            strategy.Execute();

            backtestResult = CreateBacktestResult(strategy);
            
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
        var backtestResults = await backtestResultRepository.GetAsync(algoConfigResource.BacktestResultFilterResource);
        
        var tickersInStrategiSignals = (await strategySignalRepository.GetAllAsync()).Select(x => x.Ticker).ToList();
        var tickersInBacktestResults = backtestResults.Select(x => x.Ticker).Distinct().ToList();

        foreach (var ticker in tickersInStrategiSignals)
        {
            if (!tickersInBacktestResults.Contains(ticker))
                await strategySignalRepository.UpdatePositionAsync(ticker, 0, 0.0, 0, 0.0);
        }

        foreach (var ticker in tickersInBacktestResults)
        {
            try
            {
                if (!tickersInStrategiSignals.Contains(ticker))
                    await strategySignalRepository.AddAsync(new StrategySignal { Ticker = ticker, CountSignals = 0, LastPrice = 0.0, PositionCost = 0.0, PositionSize = 0 });
                
                int countSignals = 0;
                double positionSize = 0.0;
            
                foreach (var backtestResult in backtestResults.Where(x => x.Ticker == ticker))
                {
                    if (backtestResult.CurrentPosition > 0)
                    {
                        countSignals++;
                        double positionPrice = backtestResult.CurrentPositionCost / backtestResult.CurrentPosition;
                        positionSize += algoConfigResource.MoneyManagementResource.UnitSize / positionPrice;
                    }

                    else if (backtestResult.CurrentPosition < 0)
                    {
                        countSignals--;
                        double positionPrice = backtestResult.CurrentPositionCost / backtestResult.CurrentPosition;
                        positionSize -= algoConfigResource.MoneyManagementResource.UnitSize / positionPrice;
                    }
                }
            
                double positionCost = countSignals * algoConfigResource.MoneyManagementResource.UnitSize;
                
                // Применяем плечо и получаем последнюю цену
                var sharesTickers = (await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.AlgoShares)).Select(x => x.Ticker).ToList();
                var futuresTickers = (await tickerListUtilService.GetFuturesByTickerListAsync(KnownTickerLists.AlgoFutures)).Select(x => x.Ticker).ToList();

                double lastPrice = 0.0;
                
                if (sharesTickers.Contains(ticker))
                {
                    positionSize *= algoConfigResource.MoneyManagementResource.ShareLeverage;
                    lastPrice = (await shareRepository.GetAsync(ticker))!.LastPrice;
                }

                if (futuresTickers.Contains(ticker))
                {
                    positionSize *= algoConfigResource.MoneyManagementResource.FutureLeverage;
                    lastPrice = (await futureRepository.GetAsync(ticker))!.LastPrice;
                }

                await strategySignalRepository.UpdatePositionAsync(ticker, countSignals, positionCost, Convert.ToInt32(positionSize), lastPrice);
            }
            
            catch (Exception exception)
            {
                logger.Error($"Ошибка CalculateStrategySignalsAsync '{ticker}', '{exception.Message}'");
            }
        }

        return true;
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

                        strategy.Execute();

                        var optimizationResult = CreateOptimizationResult(strategy);
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
    
    private async Task InitBacktestAsync(string? ticker = null, Guid? strategyId = null)
    {
        _isOptimization = false;
        
        // Читаем настройки из ресурсов
        _algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        _algoStrategyResources = await resourceStoreService.GetAlgoStrategiesAsync();
        
        await InitDailyCandlesAsync(ticker);
        await InitHourlyCandlesAsync(ticker);
        
        InitStrategies(strategyId);
    }
    
    private async Task InitOptimizationAsync()
    {
        _isOptimization = true;
        
        // Читаем настройки из ресурсов
        _algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        _algoStrategyResources = await resourceStoreService.GetAlgoStrategiesAsync();

        await InitDailyCandlesAsync();
        await InitHourlyCandlesAsync();
        
        InitStrategies();
    }

    private void InitStrategies(Guid? strategyId = null)
    {
        var algoStrategyResources = strategyId is null 
            ? _algoStrategyResources 
            : _algoStrategyResources.Where(x => x.Id == strategyId);
        
        foreach (var algoStrategyResource in algoStrategyResources)
        {
            var strategy = serviceProvider.GetRequiredKeyedService<Strategy>(algoStrategyResource.Name);
                
            strategy.StrategyId = algoStrategyResource.Id;
            strategy.Timeframe = algoStrategyResource.Timeframe;
            strategy.StrategyDescription = algoStrategyResource.Description;
            strategy.StrategyName = algoStrategyResource.Name;
                
            StrategyDictionary.TryAdd(algoStrategyResource.Id, strategy);
        }
    }

    private async Task InitDailyCandlesAsync(string? ticker = null)
    {
        var dates = GetDailyDates();

        var tickers = ticker is null ? await GetAllTickers() : [ticker];
        
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

    private async Task InitHourlyCandlesAsync(string? ticker = null)
    {
        var dates = GetHourlyDates();
        
        var tickers = ticker is null ? await GetAllTickers() : [ticker];
        
        foreach (string instrumentTicker in tickers)
        {
            var candles = (await hourlyCandleRepository.GetAsync(instrumentTicker, dates.From, dates.To))
                .Select(AlgoMapper.Map).ToList();

            if (candles.Count == 0)
                continue;
            
            for (int i = 0; i < candles.Count; i++)
                candles[i].Index = i;            
            
            HourlyCandles.TryAdd(instrumentTicker, candles);
        }
    }

    private async Task<List<string>> GetAllTickers()
    {
        var tickers = new List<string>();

        foreach (var algoStrategyResource in _algoStrategyResources)
        {
            var tickersInTickerList = (await resourceStoreService.GetTickerListAsync(algoStrategyResource.TickerList)).Tickers;

            foreach (var ticker in tickersInTickerList.Where(ticker => !tickers.Contains(ticker))) 
                tickers.Add(ticker);
        }
        
        return tickers;
    }

    private (DateOnly From, DateOnly To) GetDailyDates()
    {
        DateOnly from;
        DateOnly to;
        
        var today = DateOnly.FromDateTime(DateTime.Today);
        
        if (_isOptimization)
        {
            from = today
                .AddDays(-1 * _algoConfigResource.PeriodConfigResource.BacktestWindowInDays)
                .AddDays(-1 * _algoConfigResource.PeriodConfigResource.DailyStabilizationPeriodInDays)
                .AddDays(-1 * _algoConfigResource.PeriodConfigResource.BacktestShiftInDays);
            
            to = today.AddDays(-1 * _algoConfigResource.PeriodConfigResource.BacktestShiftInDays);
        }

        else
        {
            from = today
                .AddDays(-1 * _algoConfigResource.PeriodConfigResource.BacktestWindowInDays)
                .AddDays(-1 * _algoConfigResource.PeriodConfigResource.DailyStabilizationPeriodInDays);
            
            to = today;
        }

        return (from, to);
    }
    
    private (DateOnly From, DateOnly To) GetHourlyDates()
    {
        DateOnly from;
        DateOnly to;
        
        var today = DateOnly.FromDateTime(DateTime.Today);
        
        if (_isOptimization)
        {
            from = today
                .AddDays(-1 * _algoConfigResource.PeriodConfigResource.BacktestWindowInDays)
                .AddDays(-1 * _algoConfigResource.PeriodConfigResource.HourlyStabilizationPeriodInDays)
                .AddDays(-1 * _algoConfigResource.PeriodConfigResource.BacktestShiftInDays);
            
            to = today.AddDays(-1 * _algoConfigResource.PeriodConfigResource.BacktestShiftInDays);
        }

        else
        {
            from = today
                .AddDays(-1 * _algoConfigResource.PeriodConfigResource.BacktestWindowInDays)
                .AddDays(-1 * _algoConfigResource.PeriodConfigResource.HourlyStabilizationPeriodInDays);
            
            to = today;
        }
        
        return (from, to);
    }
}