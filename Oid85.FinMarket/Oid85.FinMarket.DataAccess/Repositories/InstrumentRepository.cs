using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class InstrumentRepository(
    FinMarketContext context) 
    : IInstrumentRepository
{
    public async Task AddOrUpdateAsync(List<Instrument> tickers)
    {
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

    public async Task<List<Instrument>> GetAllAsync() =>
        (await context.InstrumentEntities
            .OrderBy(x => x.Name)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();
    
    public async Task<Instrument?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.InstrumentEntities
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);
        return entity is null ? null : DataAccessMapper.Map(entity);
    }
    
    public async Task<Instrument?> GetByNameAsync(string name)
    {
        var entity = await context.InstrumentEntities
            .FirstOrDefaultAsync(x => x.Name == name);
        return entity is null ? null : DataAccessMapper.Map(entity);
    }
    
    public async Task<Instrument?> GetByTickerAsync(string ticker)
    {
        var entity = await context.InstrumentEntities
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        return entity is null ? null : DataAccessMapper.Map(entity);
    }
}