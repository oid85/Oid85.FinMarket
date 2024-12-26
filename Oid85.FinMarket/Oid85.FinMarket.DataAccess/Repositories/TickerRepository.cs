using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class TickerRepository(
    FinMarketContext context) 
    : ITickerRepository
{
    public async Task AddOrUpdateAsync(List<Ticker> tickers)
    {
        if (tickers.Count == 0)
            return;
        
        foreach (var ticker in tickers)
        {
            var entity = context.TickerEntities
                .FirstOrDefault(x => 
                    x.InstrumentId == ticker.InstrumentId);

            if (entity is null)
            {
                entity = ticker.Adapt<TickerEntity>();
                await context.TickerEntities.AddAsync(entity);
            }

            else
            {
                entity.Adapt(ticker);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<Ticker>> GetAllAsync() =>
        (await context.TickerEntities
            .OrderBy(x => x.Name)
            .ToListAsync())
        .Select(x => x.Adapt<Ticker>())
        .ToList();

    public async Task<Ticker?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.TickerEntities
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);
        
        return entity?.Adapt<Ticker>();
    }
    
    public async Task<Ticker?> GetByNameAsync(string name)
    {
        var entity = await context.TickerEntities
            .FirstOrDefaultAsync(x => x.Name == name);
        
        return entity?.Adapt<Ticker>();
    }
}