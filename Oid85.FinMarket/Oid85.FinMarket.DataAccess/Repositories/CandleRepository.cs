using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class CandleRepository(
    FinMarketContext context) 
    : ICandleRepository
{
    public async Task AddOrUpdateAsync(List<Candle> candles)
    {
        if (candles.Count == 0)
            return;
        
        var lastCandle = await GetLastAsync(candles.First().InstrumentId);

        if (lastCandle is null)
        {
            var entities = candles
                .Select(x => x.Adapt<CandleEntity>());
            await context.CandleEntities.AddRangeAsync(entities);
        }
        
        else
        {
            if (!lastCandle.IsComplete)
            {
                var candle = candles.First(x => x.Date == lastCandle.Date);
                lastCandle.Adapt(candle);
            }

            var entities = candles
                .Select(x => x.Adapt<CandleEntity>())
                .Where(x => x.Date > lastCandle.Date);
                
            await context.CandleEntities.AddRangeAsync(entities);  
        }

        await context.SaveChangesAsync();
    }

    public Task<List<Candle>> GetAsync(Guid instrumentId) =>
        context.CandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .OrderBy(x => x.Date)
            .Select(x => x.Adapt<Candle>())
            .ToListAsync();

    public async Task<Candle?> GetLastAsync(Guid instrumentId)
    {
        bool exists = await context.CandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .AnyAsync();

        if (!exists)
            return null;
        
        var maxDate = await context.CandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .MaxAsync(x => x.Date);

        var entity = await context.CandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .FirstAsync(x => x.Date == maxDate);

        var candle = entity.Adapt<Candle>();
        
        return candle;
    }
}
