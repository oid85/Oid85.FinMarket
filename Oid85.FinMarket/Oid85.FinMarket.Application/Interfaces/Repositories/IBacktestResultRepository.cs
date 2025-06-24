using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IBacktestResultRepository
{
    Task AddAsync(List<BacktestResult> backtestResults);
    Task<List<BacktestResult>> GetAsync(BacktestResultFilterResource filter);
    Task<BacktestResult?> GetAsync(Guid id);
    Task DeleteAsync(Guid strategyId);
    Task InvertDeleteAsync(List<Guid> strategyIds);
}