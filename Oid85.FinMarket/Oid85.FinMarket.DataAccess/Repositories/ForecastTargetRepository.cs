using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ForecastTargetRepository(
    IDbContextFactory<FinMarketContext> contextFactory)
    : IForecastTargetRepository
{
    public async Task AddAsync(List<ForecastTarget> forecastTargets)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        if (forecastTargets is [])
            return;

        var entities = new List<ForecastTargetEntity>();
        
        foreach (var forecastTarget in forecastTargets)
            if (!await context.ForecastTargetEntities
                    .AnyAsync(x => 
                        x.InstrumentId == forecastTarget.InstrumentId && 
                        x.Company == forecastTarget.Company && 
                        x.RecommendationDate == forecastTarget.RecommendationDate))
                entities.Add(DataAccessMapper.Map(forecastTarget));

        await context.ForecastTargetEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
    
    public async Task<List<ForecastTarget>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.ForecastTargetEntities
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }

    public async Task<List<ForecastTarget>> GetAsync(List<Guid> instrumentIds)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.ForecastTargetEntities
                .Where(x => instrumentIds.Contains(x.InstrumentId))
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }
}