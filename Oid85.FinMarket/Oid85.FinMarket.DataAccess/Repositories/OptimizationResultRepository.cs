using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class OptimizationResultRepository(
    FinMarketContext context) 
    : IOptimizationResultRepository
{
    public async Task AddAsync(List<OptimizationResult> optimizationResults)
    {
        if (optimizationResults is [])
            return;

        var entities = optimizationResults.Select(DataAccessMapper.Map);
        
        await context.OptimizationResultEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
}