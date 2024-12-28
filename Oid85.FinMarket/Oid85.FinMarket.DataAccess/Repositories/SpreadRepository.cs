using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class SpreadRepository(
    FinMarketContext context) 
    : ISpreadRepository
{
    public async Task AddOrUpdateAsync(List<Spread> spreads)
    {
        if (spreads.Count == 0)
            return;
        
        foreach (var spread in spreads)
        {
            var entity = context.SpreadEntities
                .FirstOrDefault(x => 
                    x.FirstInstrumentId == spread.FirstInstrumentId);

            if (entity is null)
            {
                entity = spread.Adapt<SpreadEntity>();
                await context.SpreadEntities.AddAsync(entity);
            }

            else
            {
                entity.Adapt(spread);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<Spread>> GetAllAsync() =>
        (await context.SpreadEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.FirstInstrumentTicker)
            .ToListAsync())
        .Select(x => x.Adapt<Spread>())
        .ToList();

    public async Task<List<Spread>> GetWatchListAsync() =>
        (await context.SpreadEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.FirstInstrumentTicker)
            .ToListAsync())
        .Select(x => x.Adapt<Spread>())
        .ToList();

    public async Task<Spread?> GetByTickerAsync(string firstInstrumentTicker)
    {
        var entity = await context.SpreadEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.FirstInstrumentTicker == firstInstrumentTicker);
        
        return entity?.Adapt<Spread>();
    }
}
