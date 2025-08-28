using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Services;

public interface IAlgoStatisticalArbitrageService
{
    /// <summary>
    /// Рассчитать корреляции
    /// </summary>
    Task CalculateCorrelationAsync();
    
    /// <summary>
    /// Рассчитать хвосты регрессии
    /// </summary>
    Task<Dictionary<string, RegressionTail>> CalculateRegressionTailsAsync();
    
    /// <summary>
    /// Выполнить бэктест
    /// </summary>
    Task<bool> BacktestAsync();
    
    /// <summary>
    /// Выполнить бэктест по Id
    /// </summary>
    Task<(StatisticalArbitrageBacktestResult? backtestResult, StatisticalArbitrageStrategy? strategy)> BacktestAsync(Guid backtestResultId);
    
    /// <summary>
    /// Рассчитать сигналы
    /// </summary>
    Task<bool> CalculateStrategySignalsAsync();
    
    /// <summary>
    /// Выполнить оптимизацию
    /// </summary>
    Task<bool> OptimizeAsync();
}