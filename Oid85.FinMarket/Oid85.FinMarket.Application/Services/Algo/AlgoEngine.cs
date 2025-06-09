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

public class AlgoEngine(
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
    public async Task InitBacktestAsync()
    {
        _isOptimization = false;
        
        // Читаем настройки из ресурсов
        _algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        _algoStrategyResources = await resourceStoreService.GetAlgoStrategiesAsync();
        
        await InitDailyCandlesAsync();
        await InitHourlyCandlesAsync();
        
        InitStrategies();
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

    private void InitStrategies()
    {
        foreach (var algoStrategyResource in _algoStrategyResources)
        {
            var strategy = serviceProvider.GetRequiredKeyedService<Strategy>(algoStrategyResource.Name);
            StrategyDictionary.TryAdd(algoStrategyResource.Id, strategy);
        }
    }

    private async Task InitDailyCandlesAsync()
    {
        var dates = GetDailyDates();

        foreach (string ticker in _algoConfigResource.Tickers)
        {
            var candles = (await dailyCandleRepository.GetAsync(ticker, dates.From, dates.To))
                .Select(AlgoMapper.Map).ToList();
            
            if (candles.Count == 0)
                continue;

            for (int i = 0; i < candles.Count; i++)
                candles[i].Index = i;
            
            DailyCandles.TryAdd(ticker, candles);
        }
    }

    private async Task InitHourlyCandlesAsync()
    {
        var dates = GetHourlyDates();
        
        foreach (string ticker in _algoConfigResource.Tickers)
        {
            var candles = (await hourlyCandleRepository.GetAsync(ticker, dates.From, dates.To))
                .Select(AlgoMapper.Map).ToList();

            if (candles.Count == 0)
                continue;
            
            for (int i = 0; i < candles.Count; i++)
                candles[i].Index = i;            
            
            HourlyCandles.TryAdd(ticker, candles);
        }
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