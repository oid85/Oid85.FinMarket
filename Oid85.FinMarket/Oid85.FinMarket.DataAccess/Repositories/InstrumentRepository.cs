using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class InstrumentRepository(
    FinMarketContext context) 
    : IInstrumentRepository
{
    public async Task AddOrUpdateAsync(List<Instrument> tickers)
    {
        if (tickers.Count == 0)
            return;
        
        foreach (var ticker in tickers)
        {
            var entity = context.InstrumentEntities
                .FirstOrDefault(x => 
                    x.InstrumentId == ticker.InstrumentId);

            if (entity is null)
            {
                entity = ticker.Adapt<InstrumentEntity>();
                await context.InstrumentEntities.AddAsync(entity);
            }

            else
            {
                entity.Adapt(ticker);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<Instrument>> GetAllAsync() =>
        (await context.InstrumentEntities
            .OrderBy(x => x.Name)
            .ToListAsync())
        .Select(x => x.Adapt<Instrument>())
        .ToList();

    public async Task<Instrument?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.InstrumentEntities
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);
        
        return entity?.Adapt<Instrument>();
    }
    
    public async Task<Instrument?> GetByNameAsync(string name)
    {
        var entity = await context.InstrumentEntities
            .FirstOrDefaultAsync(x => x.Name == name);
        
        return entity?.Adapt<Instrument>();
    }
}