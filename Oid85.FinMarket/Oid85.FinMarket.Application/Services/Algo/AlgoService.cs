using System.Collections.Concurrent;
using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Models;
using Oid85.FinMarket.Domain.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.Application.Services.Algo;

public class AlgoService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IHourlyCandleRepository hourlyCandleRepository,
    IResourceStoreService resourceStoreService,
    IServiceProvider serviceProvider)
{
    public ConcurrentDictionary<string, List<Candle>> DailyCandles { get; set; } = new();
    public ConcurrentDictionary<string, List<Candle>> HourlyCandles { get; set; } = new();
    public ConcurrentDictionary<Guid, Strategy> StrategyDictionary { get; set; } = new();

    private AlgoConfigResource _algoConfigResource = new();
    private List<AlgoStrategyResource> _algoStrategyResources = new();
    
    private bool _isOptimization;
    
    /// <summary>
    /// Инициализация AlgoEngine для бэктеста
    /// </summary>
    public async Task InitBacktestAsync(string? ticker = null, Guid? strategyId = null)
    {
        _isOptimization = false;
        
        // Читаем настройки из ресурсов
        _algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        _algoStrategyResources = await resourceStoreService.GetAlgoStrategiesAsync();
        
        await InitDailyCandlesAsync(ticker);
        await InitHourlyCandlesAsync(ticker);
        
        InitStrategies(strategyId);
    }

    /// <summary>
    /// Инициализация AlgoEngine для оптимизации
    /// </summary>
    public async Task InitOptimizationAsync()
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