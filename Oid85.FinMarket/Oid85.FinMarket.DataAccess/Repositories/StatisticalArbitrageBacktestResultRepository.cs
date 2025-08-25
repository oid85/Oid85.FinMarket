using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class StatisticalArbitrageBacktestResultRepository(
    IDbContextFactory<FinMarketContext> contextFactory)
    : IStatisticalArbitrageBacktestResultRepository
{
    public async Task AddAsync(List<StatisticalArbitrageBacktestResult> backtestResults)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        if (backtestResults is [])
            return;

        var entities = backtestResults.Select(DataAccessMapper.Map);
        
        await context.StatisticalArbitrageBacktestResultEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task<List<StatisticalArbitrageBacktestResult>> GetAsync(BacktestResultFilterResource filter)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var queryableEntities = context.StatisticalArbitrageBacktestResultEntities.AsQueryable();
        
        queryableEntities = queryableEntities.Where(x => x.ProfitFactor >= filter.MinProfitFactor);
        queryableEntities = queryableEntities.Where(x => x.RecoveryFactor >= filter.MinRecoveryFactor);
        queryableEntities = queryableEntities.Where(x => x.WinningTradesPercent >= filter.MinWinningTradesPercent);
        queryableEntities = queryableEntities.Where(x => x.WinningTradesPercent <= filter.MaxWinningTradesPercent);
        queryableEntities = queryableEntities.Where(x => x.AnnualYieldReturn >= filter.MinAnnualYieldReturn);
        queryableEntities = queryableEntities.Where(x => x.MaxDrawdownPercent <= filter.MaxDrawdownPercent);
        
        var entities = await queryableEntities.AsNoTracking().ToListAsync();
        
        var models = entities.Select(DataAccessMapper.Map).ToList();

        return models;
    }

    public async Task<StatisticalArbitrageBacktestResult?> GetAsync(Guid backtestResultId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var entity = await context.StatisticalArbitrageBacktestResultEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == backtestResultId);
        
        return entity is null ? null : DataAccessMapper.Map(entity);
    }

    public async Task DeleteAsync(Guid strategyId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await context.StatisticalArbitrageBacktestResultEntities.Where(x => x.StrategyId == strategyId).ExecuteDeleteAsync();
    }

    public async Task InvertDeleteAsync(List<Guid> strategyIds)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await context.StatisticalArbitrageBacktestResultEntities.Where(x => !strategyIds.Contains(x.StrategyId)).ExecuteDeleteAsync();
    }
}