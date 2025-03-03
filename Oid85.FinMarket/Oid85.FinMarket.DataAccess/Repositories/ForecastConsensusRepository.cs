using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ForecastConsensusRepository(
    FinMarketContext context) 
    : IForecastConsensusRepository
{
    public async Task AddAsync(List<ForecastConsensus> forecastConsensuses)
    {
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
    
    public async Task<List<ForecastConsensus>> GetAllAsync() =>
        (await context.ForecastConsensusEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();
}