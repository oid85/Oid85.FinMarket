using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class CandleRepository(
    FinMarketContext context) 
    : ICandleRepository
{
    public async Task AddOrUpdateAsync(List<Candle> candles)
    {
        if (candles is [])
            return;
        
        var lastCandle = await GetLastAsync(candles.First().InstrumentId);

        if (lastCandle is null)
        {
            var entities = candles.Select(DataAccessMapper.Map);
            await context.CandleEntities.AddRangeAsync(entities);
        }
        
        else
        {
            if (!lastCandle.IsComplete)
            {
                var candle = candles.Find(x => x.Date == lastCandle.Date);

                if (candle is not null)
                {
                    var entity = await context.CandleEntities
                        .FirstAsync(x => 
                            x.Date == candle.Date &&
                            x.InstrumentId == candle.InstrumentId);

                    DataAccessMapper.Map(ref entity, candle);
                }
            }

            var entities = candles
                .Select(DataAccessMapper.Map)
                .Where(x => x.Date > lastCandle.Date);
                
            await context.CandleEntities.AddRangeAsync(entities);  
        }

        await context.SaveChangesAsync();
    }
    
    public async Task<Candle?> GetLastAsync(Guid instrumentId)
    {
        var entity = await context.CandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .OrderByDescending(x => x.Date)
            .Take(1)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return entity is null ? null : DataAccessMapper.Map(entity);
    }

    public async Task<List<Candle>> GetLastYearAsync(Guid instrumentId)
    {
        var from = DateOnly.FromDateTime(DateTime.Today.AddYears(-1));
        var to = DateOnly.FromDateTime(DateTime.Today);
        
        var entities = await context.CandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .Where(x => 
                x.Date >= from &&
                x.Date <= to)
            .OrderBy(x => x.Date)
            .AsNoTracking()
            .ToListAsync();

        return entities.Count == 0 ? [] : entities.Select(DataAccessMapper.Map).ToList();
    }

    public async Task<Candle?> GetAsync(Guid instrumentId, DateOnly date)
    {
        var entity = await context.CandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .Where(x => x.Date == date)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return entity is null ? null : DataAccessMapper.Map(entity);
    }
}
