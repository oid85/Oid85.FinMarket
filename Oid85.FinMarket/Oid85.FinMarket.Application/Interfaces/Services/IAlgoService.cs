using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Services;

public interface IAlgoService
{
    Task<bool> BacktestAsync();
    Task<(BacktestResult? backtestResult, Strategy? strategy)> BacktestAsync(Guid backtestResultId);
    Task<bool> CalculateStrategySignalsAsync();
    Task<bool> OptimizeAsync();
}