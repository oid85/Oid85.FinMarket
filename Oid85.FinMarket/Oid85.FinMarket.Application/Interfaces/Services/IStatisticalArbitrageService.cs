using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Services;

public interface IStatisticalArbitrageService
{
    /// <summary>
    /// Рассчитать корреляции
    /// </summary>
    Task CalculateCorrelationAsync();
    
    /// <summary>
    /// Рассчитать хвосты регрессии
    /// </summary>
    Task<Dictionary<string, RegressionTail>> CalculateRegressionTailsAsync();
    
    Task<bool> BacktestAsync();
    Task<bool> CalculateStrategySignalsAsync();
    Task<bool> OptimizeAsync();
}