using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Services.Algo;

public interface IBacktestService
{
    Task<bool> BacktestAsync();
    Task<(BacktestResult? backtestResult, Strategy? strategy)> BacktestAsync(Guid backtestResultId);
}