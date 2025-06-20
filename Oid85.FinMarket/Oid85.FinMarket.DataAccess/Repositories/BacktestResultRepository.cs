using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class BacktestResultRepository(
    FinMarketContext context)  
    : IBacktestResultRepository
{
    public async Task AddAsync(List<BacktestResult> backtestResults)
    {
        if (backtestResults is [])
            return;

        var entities = backtestResults.Select(DataAccessMapper.Map);
        
        await context.BacktestResultEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task<List<BacktestResult>> GetAsync(BacktestResultFilterResource filter) =>
        (await context.BacktestResultEntities
            .Where(x => 
                x.ProfitFactor >= filter.ProfitFactor &&
                x.RecoveryFactor >= filter.RecoveryFactor &&
                x.MaxDrawdownPercent <= filter.MaxDrawdownPercent)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();

    public async Task<BacktestResult?> GetAsync(Guid id)
    {
        var entity = await context.BacktestResultEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        
        return entity is null ? null : DataAccessMapper.Map(entity);
    }

    public async Task<List<BacktestResult>> GetGoodAsync()
    {
        var queryableEntities = context.BacktestResultEntities.AsQueryable();
        
        queryableEntities = queryableEntities.Where(x => x.ProfitFactor >= 2.0);
        queryableEntities = queryableEntities.Where(x => x.RecoveryFactor >= 2.0);
        queryableEntities = queryableEntities.Where(x => x.MaxDrawdownPercent <= 20.0);
        queryableEntities = queryableEntities.Where(x => x.AnnualYieldReturn >= 30.0);
        queryableEntities = queryableEntities.Where(x => x.WinningTradesPercent >= 60.0);
        queryableEntities = queryableEntities.Where(x => x.WinningTradesPercent <= 90.0);
        
        var entities = await queryableEntities.AsNoTracking().ToListAsync();
        
        var models = entities.Select(DataAccessMapper.Map).ToList();

        return models;
    }

    public Task DeleteAsync(Guid strategyId) => 
        context.BacktestResultEntities.Where(x => x.StrategyId == strategyId).ExecuteDeleteAsync();
    
    public Task InvertDeleteAsync(List<Guid> strategyIds) => 
        context.BacktestResultEntities.Where(x => !strategyIds.Contains(x.StrategyId)).ExecuteDeleteAsync();
}