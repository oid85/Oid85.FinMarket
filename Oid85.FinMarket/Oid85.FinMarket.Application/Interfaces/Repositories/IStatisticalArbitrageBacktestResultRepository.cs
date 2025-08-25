using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IStatisticalArbitrageBacktestResultRepository
{
    Task AddAsync(List<StatisticalArbitrageBacktestResult> backtestResults);
    Task<List<StatisticalArbitrageBacktestResult>> GetAsync(BacktestResultFilterResource filter);
    Task<StatisticalArbitrageBacktestResult?> GetAsync(Guid backtestResultId);
    Task DeleteAsync(Guid strategyId);
    Task InvertDeleteAsync(List<Guid> strategyIds);
}