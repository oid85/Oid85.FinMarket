using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

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

    public async Task<List<OptimizationResult>> GetAsync(OptimizationResultFilterResource filter) =>
        (await context.OptimizationResultEntities
            .Where(x => 
                x.ProfitFactor >= filter.ProfitFactor &&
                x.RecoveryFactor >= filter.RecoveryFactor &&
                x.MaxDrawdownPercent <= filter.MaxDrawdownPercent)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();

    public async Task<List<OptimizationResult>> GetGoodAsync()
    {
        var queryableEntities = context.OptimizationResultEntities.AsQueryable();
        
        // Фильтрация по ProfitFactor
        queryableEntities = queryableEntities.Where(x => x.ProfitFactor >= 2.0);
        
        // Фильтрация по RecoveryFactor
        queryableEntities = queryableEntities.Where(x => x.RecoveryFactor >= 2.0);
        
        // Фильтрация по MaxDrawdownPercent
        queryableEntities = queryableEntities.Where(x => x.MaxDrawdownPercent <= 20.0);
        
        var entities = await queryableEntities.AsNoTracking().ToListAsync();
        
        var models = entities.Select(DataAccessMapper.Map).ToList();

        return models;
    }

    public Task DeleteAsync(Guid strategyId) => 
        context.OptimizationResultEntities.Where(x => x.StrategyId == strategyId).ExecuteDeleteAsync();

    public Task InvertDeleteAsync(List<Guid> strategyIds) => 
        context.OptimizationResultEntities.Where(x => !strategyIds.Contains(x.StrategyId)).ExecuteDeleteAsync();
}