using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class FiveMinuteCandleRepository(
    FinMarketContext context) 
    : IFiveMinuteCandleRepository
{
    public async Task AddOrUpdateAsync(List<FiveMinuteCandle> candles)
    {
        var completedCandles = candles
            .Where(x => x.IsComplete).ToList();
        
        if (completedCandles is [])
            return;

        var entities = new List<FiveMinuteCandleEntity>();
        
        foreach (var candle in completedCandles)
            if (!await context.FiveMinuteCandleEntities
                    .AnyAsync(x => 
                        x.InstrumentId == candle.InstrumentId && 
                        x.Date == candle.Date &&
                        x.Time == candle.Time))
                entities.Add(DataAccessMapper.Map(candle));

        await context.FiveMinuteCandleEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
    
    public async Task<FiveMinuteCandle?> GetLastAsync(Guid instrumentId)
    {
        bool exists = await context.FiveMinuteCandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .AsNoTracking()
            .AnyAsync();

        if (!exists)
            return null;
        
        var maxDate = await context.FiveMinuteCandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .AsNoTracking()
            .MaxAsync(x => x.Date);

        var entitiesByMaxDate = await context.FiveMinuteCandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .Where(x => x.Date == maxDate)
            .AsNoTracking()
            .ToListAsync();

        var lastCandleEntity = entitiesByMaxDate.OrderBy(x => x.Time).Last();
        
        return DataAccessMapper.Map(lastCandleEntity);
    }

    public async Task<List<FiveMinuteCandle>> GetLastWeekCandlesAsync(Guid instrumentId)
    {
        var from = DateTime.UtcNow.AddDays(-7);
        var to = DateTime.UtcNow;
        
        var entities = await context.FiveMinuteCandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .Where(x => 
                x.DateTime >= from &&
                x.DateTime <= to)
            .AsNoTracking()
            .ToListAsync();

        return entities.Count == 0 ? [] : entities
            .Select(DataAccessMapper.Map)
            .OrderBy(x => x.DateTime)
            .ToList();
    }

    public async Task<List<FiveMinuteCandle>> GetAsync(Guid instrumentId, DateTime from, DateTime to)
    {
        var entities = await context.FiveMinuteCandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .Where(x => 
                x.Date >= DateOnly.FromDateTime(from) &&
                x.Date <= DateOnly.FromDateTime(to))
            .AsNoTracking()
            .ToListAsync();

        return entities.Count == 0 ? [] : entities
            .Select(DataAccessMapper.Map)
            .Where(x => 
                x.DateTime >= from &&
                x.DateTime <= to)
            .OrderBy(x => x.DateTime)
            .ToList();
    }
}