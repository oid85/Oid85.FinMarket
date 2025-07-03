using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class InstrumentRepository(
    IDbContextFactory<FinMarketContext> contextFactory)
    : IInstrumentRepository
{
    public async Task AddOrUpdateAsync(List<Instrument> tickers)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        if (tickers is [])
            return;
        
        foreach (var ticker in tickers)
        {
            var entity = context.InstrumentEntities
                .FirstOrDefault(x => 
                    x.InstrumentId == ticker.InstrumentId);

            if (entity is null)
            {
                DataAccessMapper.Map(ref entity, ticker);
                
                if (entity is not null)
                    await context.InstrumentEntities.AddAsync(entity);
            }

            else
                DataAccessMapper.Map(ref entity, ticker);
        }

        await context.SaveChangesAsync();
    }
    
    public async Task<Instrument?> GetAsync(Guid instrumentId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var entity = await context.InstrumentEntities
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);
        return entity is null ? null : DataAccessMapper.Map(entity);
    }

    public async Task<List<Instrument>> GetAsync(List<Guid> instrumentIds)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.InstrumentEntities
                .Where(x => instrumentIds.Contains(x.InstrumentId))
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }

    public async Task<Instrument?> GetAsync(string ticker)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var entity = await context.InstrumentEntities
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null ? null : DataAccessMapper.Map(entity);
    }
}