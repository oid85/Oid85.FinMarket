using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IOptimizationResultRepository
{
    Task AddAsync(List<OptimizationResult> optimizationResults);
}