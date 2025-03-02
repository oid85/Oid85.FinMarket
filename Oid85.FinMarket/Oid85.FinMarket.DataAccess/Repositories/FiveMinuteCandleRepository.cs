using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class FiveMinuteCandleRepository(
    FinMarketContext context) 
    : IFiveMinuteCandleRepository
{
    public async Task AddOrUpdateAsync(List<FiveMinuteCandle> candles)
    {
        if (candles is [])
            return;
        
        var lastCandle = await GetLastAsync(candles.First().InstrumentId);

        if (lastCandle is null)
        {
            var entities = candles.Select(DataAccessMapper.Map);
            await context.FiveMinuteCandleEntities.AddRangeAsync(entities);
        }
        
        else
        {
            if (!lastCandle.IsComplete)
            {
                var candle = candles.Find(x => 
                    x.Date == lastCandle.Date &&
                    x.Time == lastCandle.Time);

                if (candle is not null)
                {
                    var entity = await context.FiveMinuteCandleEntities
                        .FirstAsync(x => 
                            x.Date == candle.Date &&
                            x.Time == lastCandle.Time &&
                            x.InstrumentId == candle.InstrumentId);

                    DataAccessMapper.Map(ref entity, candle);
                }
            }

            var entities = candles
                .Select(DataAccessMapper.Map)
                .Where(x => 
                    x.Date.ToDateTime(x.Time) > lastCandle.Date.ToDateTime(x.Time));
                
            await context.FiveMinuteCandleEntities.AddRangeAsync(entities);  
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<FiveMinuteCandle>> GetAsync(Guid instrumentId) =>
        (await context.FiveMinuteCandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .OrderBy(x => x.Date.ToDateTime(x.Time))
        .ToList();

    public async Task<List<FiveMinuteCandle>> GetAsync(
        Guid instrumentId, DateTime from, DateTime to)
    {
        var candles = await GetAsync(instrumentId);
        
        return candles
            .Where(x => 
                x.Date.ToDateTime(x.Time) >= from && 
                x.Date.ToDateTime(x.Time) <= to)
            .ToList();
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
}