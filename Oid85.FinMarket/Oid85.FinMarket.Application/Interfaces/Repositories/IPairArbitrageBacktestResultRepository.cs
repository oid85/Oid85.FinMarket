using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IPairArbitrageBacktestResultRepository
{
    Task AddAsync(List<PairArbitrageBacktestResult> backtestResults);
    Task<List<PairArbitrageBacktestResult>> GetAsync(PairArbitrageBacktestResultFilterResource filter);
    Task<List<PairArbitrageBacktestResult>> GetAllAsync();
    Task<PairArbitrageBacktestResult?> GetAsync(Guid backtestResultId);
    Task DeleteAsync(Guid strategyId);
    Task InvertDeleteAsync(List<Guid> strategyIds);
}