using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Services;

public interface IAlgoService
{
    /// <summary>
    /// Выполнить бэктест
    /// </summary>
    Task<bool> BacktestAsync();
    
    /// <summary>
    /// Выполнить бэктест по Id
    /// </summary>
    Task<(BacktestResult? backtestResult, Strategy? strategy)> BacktestAsync(Guid backtestResultId);
    
    /// <summary>
    /// Рассчитать сигналы
    /// </summary>
    Task<bool> CalculateStrategySignalsAsync();
    
    /// <summary>
    /// Выполнить оптимизацию
    /// </summary>
    Task<bool> OptimizeAsync();
}