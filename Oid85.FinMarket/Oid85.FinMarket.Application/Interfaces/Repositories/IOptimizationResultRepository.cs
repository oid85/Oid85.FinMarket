using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IOptimizationResultRepository
{
    Task AddAsync(List<OptimizationResult> optimizationResults);
    Task<List<OptimizationResult>> GetAsync(OptimizationResultFilterResource filter);
    Task DeleteAsync(Guid strategyId);
    Task InvertDeleteAsync(List<Guid> strategyIds);
}