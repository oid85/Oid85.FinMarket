﻿using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class OptimizationResultRepository(
    IDbContextFactory<FinMarketContext> contextFactory)
    : IOptimizationResultRepository
{
    public async Task AddAsync(List<OptimizationResult> optimizationResults)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        if (optimizationResults is [])
            return;

        var entities = optimizationResults.Select(DataAccessMapper.Map);
        
        await context.OptimizationResultEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task<List<OptimizationResult>> GetAsync(OptimizationResultFilterResource filter)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var queryableEntities = context.OptimizationResultEntities.AsQueryable();
        
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

    public async Task DeleteAsync(Guid strategyId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await context.OptimizationResultEntities.Where(x => x.StrategyId == strategyId).ExecuteDeleteAsync();
    }

    public async Task InvertDeleteAsync(List<Guid> strategyIds)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await context.OptimizationResultEntities.Where(x => !strategyIds.Contains(x.StrategyId)).ExecuteDeleteAsync();
    }
}