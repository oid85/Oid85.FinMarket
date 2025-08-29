using Accord.Statistics.Models.Regression.Linear;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Common.Utils;
using Oid85.FinMarket.Domain.Mapping;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.Computation;
using Oid85.FinMarket.External.ResourceStore;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.Application.Helpers;

public class AlgoHelper(
    ILogger logger,
    IServiceProvider serviceProvider,
    IResourceStoreService resourceStoreService,
    IRegressionTailRepository regressionTailRepository,
    IDailyCandleRepository dailyCandleRepository,
    ICorrelationRepository correlationRepository,
    IComputationService computationService,
    IFutureRepository futureRepository)
{
    /// <summary>
    /// Получить даты для оптимизации
    /// </summary>
    private async Task<(DateOnly From, DateOnly To)> GetOptimizationDatesAsync()
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
    private async Task<(DateOnly From, DateOnly To)> GetBacktestDatesAsync()
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
    private async Task<List<string>> GetAllTickersForAlgoAsync()
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
    private async Task<List<string>> GetAllTickersForStatisticalArbitrageAsync()
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
    /// Получение свечей для Алго
    /// </summary>
    /// <param name="isOptimization">Признак выполнения процесса оптимизации</param>
    /// <param name="ticker">Тикер инструмента</param>
    public async Task<Dictionary<string, List<Candle>>> GetAlgoCandlesAsync(bool isOptimization, string? ticker = null)
    {
        var dates = isOptimization ? await GetOptimizationDatesAsync() : await GetBacktestDatesAsync();

        var tickers = ticker is null ? await GetAllTickersForAlgoAsync() : [ticker];
        
        var result = new Dictionary<string, List<Candle>>();
        
        foreach (string instrumentTicker in tickers)
        {
            var candles = (await dailyCandleRepository.GetAsync(instrumentTicker, dates.From, dates.To))
                .Select(AlgoMapper.Map).ToList();
            
            if (candles.Count == 0)
                continue;

            for (int i = 0; i < candles.Count; i++)
                candles[i].Index = i;
            
            result.TryAdd(instrumentTicker, candles);
        }

        return result;
    }
    
    /// <summary>
    /// Получение свечей для Статистического арбитража
    /// </summary>
    /// <param name="isOptimization">Признак выполнения процесса оптимизации</param>
    /// <param name="ticker1">Тикер первого инструмента</param>
    /// <param name="ticker2">Тикер второго инструмента</param>
    public async Task<Dictionary<string, List<Candle>>> GetStatisticalArbitrageCandlesAsync(bool isOptimization, string? ticker1 = null, string? ticker2 = null)
    {
        var dates = isOptimization ? await GetOptimizationDatesAsync() : await GetBacktestDatesAsync();

        var tickers = ticker1 is null || ticker2 is null 
            ? await GetAllTickersForStatisticalArbitrageAsync() 
            : [ticker1, ticker2];

        var result = new Dictionary<string, List<Candle>>();
        
        foreach (string instrumentTicker in tickers)
        {
            var candles = (await dailyCandleRepository.GetAsync(instrumentTicker, dates.From, dates.To))
                .Select(AlgoMapper.Map).ToList();

            if (candles.Count == 0)
                continue;

            for (int i = 0; i < candles.Count; i++)
                candles[i].Index = i;
            
            result.TryAdd(instrumentTicker, candles);
        }
        
        return result;
    }    
    
    /// <summary>
    /// Получить спреды
    /// </summary>
    /// <param name="ticker1">Тикер первого инструмента</param>
    /// <param name="ticker2">Тикер второго инструмента</param>
    public async Task<Dictionary<string, RegressionTail>> GetSpreadsAsync(string? ticker1 = null, string? ticker2 = null)
    {
        var tails = ticker1 is null || ticker2 is null 
            ? await regressionTailRepository.GetAllAsync()
            : (await regressionTailRepository.GetAllAsync()).Where(x => x.Ticker1 == ticker1 && x.Ticker2 == ticker2);

        var spreads = new Dictionary<string, RegressionTail>();
        
        foreach (var tail in tails)
        {
            if (tail.Tails.Count == 0)
                continue;

            spreads.TryAdd($"{tail.Ticker1},{tail.Ticker2}", tail);
        }

        return spreads;
    } 
    
    /// <summary>
    /// Получить параметры стратегии
    /// </summary>
    /// <param name="strategyParams">Параметры из ресурсов</param>
    public static List<Dictionary<string, int>> GetParameterSets(List<StrategyParamResource> strategyParams)
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
    
    /// <summary>
    /// Z-score
    /// </summary>
    private static List<RegressionTailItem> ZScore(List<RegressionTailItem> values)
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
    
    /// <summary>
    /// Синхронизация свечей
    /// </summary>
    public (List<DailyCandle> Candles1, List<DailyCandle> Candles2) SyncCandles(List<DailyCandle> candles1, List<DailyCandle> candles2)
    {
        var dates1 = candles1.Select(x => x.Date).ToList();
        var dates2 = candles2.Select(x => x.Date).ToList();

        var dates = dates1.Intersect(dates2).ToList();

        var resultCandles1 = candles1.Where(x => dates.Contains(x.Date)).OrderBy(x => x.Date).ToList();
        var resultCandles2 = candles2.Where(x => dates.Contains(x.Date)).OrderBy(x => x.Date).ToList();

        return (resultCandles1, resultCandles2);
    }

    /// <summary>
    /// Синхронизация свечей
    /// </summary>
    public (List<Candle> Candles1, List<Candle> Candles2) SyncCandles(List<Candle> candles1, List<Candle> candles2)
    {
        var dates1 = candles1.Select(x => x.DateTime.Date).ToList();
        var dates2 = candles2.Select(x => x.DateTime.Date).ToList();

        var dates = dates1.Intersect(dates2).ToList();

        var resultCandles1 = candles1.Where(x => dates.Contains(x.DateTime.Date)).OrderBy(x => x.DateTime.Date).ToList();
        var resultCandles2 = candles2.Where(x => dates.Contains(x.DateTime.Date)).OrderBy(x => x.DateTime.Date).ToList();

        return (resultCandles1, resultCandles2);
    }     
    
    /// <summary>
    /// Рассчитать хвосты регрессии за период
    /// </summary>
    public async Task<Dictionary<string, RegressionTail>> CalculateRegressionTailsAsync(DateOnly from, DateOnly to)
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
                var syncCandles = SyncCandles(
                    await dailyCandleRepository.GetAsync(correlation.Ticker1, from, to), 
                    await dailyCandleRepository.GetAsync(correlation.Ticker2, from, to));

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
                        tails.Add(key, new RegressionTail { Ticker1 = correlation.Ticker1, Ticker2 = correlation.Ticker2 });

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
                logger.Error(exception, "Ошибка расчета остатков регрессии. {ticker1}, {ticker2}", correlation.Ticker1, correlation.Ticker2);
            }
        }

        return tails;
    }
    
    /// <summary>
    /// Признак фьючерса
    /// </summary>
    /// <param name="ticker">Тикер инструмента</param>
    private async Task<bool> IsFuture(string ticker) => (await futureRepository.GetAllAsync()).Select(x => x.Ticker).Contains(ticker);

    /// <summary>
    /// Получить размер основного актива
    /// </summary>
    /// <param name="ticker">Тикер инструмента</param>
    private async Task<double> GetBasicAssetSize(string ticker) => (await futureRepository.GetAsync(ticker))?.BasicAssetSize ?? 1.0;
}