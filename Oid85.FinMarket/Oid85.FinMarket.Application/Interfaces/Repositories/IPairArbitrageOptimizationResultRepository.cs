using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IPairArbitrageOptimizationResultRepository
{
    Task AddAsync(List<PairArbitrageOptimizationResult> optimizationResults);
    Task<List<PairArbitrageOptimizationResult>> GetAsync(PairArbitrageOptimizationResultFilterResource filter);
    Task DeleteAsync(Guid strategyId);
    Task InvertDeleteAsync(List<Guid> strategyIds);
}