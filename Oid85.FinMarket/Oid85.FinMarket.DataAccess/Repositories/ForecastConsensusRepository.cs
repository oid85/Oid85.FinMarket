using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ForecastConsensusRepository(
    IDbContextFactory<FinMarketContext> contextFactory)
    : IForecastConsensusRepository
{
    public async Task AddAsync(List<ForecastConsensus> forecastConsensuses)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        if (forecastConsensuses is [])
            return;

        var entities = new List<ForecastConsensusEntity>();
        
        foreach (var forecastConsensuse in forecastConsensuses)
            if (!await context.ForecastConsensusEntities
                    .AnyAsync(x => 
                        x.InstrumentId == forecastConsensuse.InstrumentId))
                entities.Add(DataAccessMapper.Map(forecastConsensuse));

        await context.ForecastConsensusEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
    
    public async Task<List<ForecastConsensus>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.ForecastConsensusEntities
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }

    public async Task<List<ForecastConsensus>> GetAsync(List<Guid> instrumentIds)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.ForecastConsensusEntities
                .Where(x => instrumentIds.Contains(x.InstrumentId))
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }
}