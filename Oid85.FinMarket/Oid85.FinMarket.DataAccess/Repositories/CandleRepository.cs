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

    public async Task<List<Candle>> GetAsync(Guid instrumentId) =>
        (await context.CandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .OrderBy(x => x.Date)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();

    public async Task<Candle?> GetLastAsync(Guid instrumentId)
    {
        var entity = await context.CandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .OrderByDescending(x => x.Date)
            .Take(1)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (entity is null)
            return null;
        
        var model = DataAccessMapper.Map(entity);
        
        return model;
    }

    public async Task<List<Candle>> GetTwoLastAsync(Guid instrumentId)
    {
        var entities = await context.CandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .OrderByDescending(x => x.Date)
            .Take(2)
            .OrderBy(x => x.Date)
            .AsNoTracking()
            .ToListAsync();

        if (entities.Count < 2)
            return [];
        
        var models = entities.Select(DataAccessMapper.Map).ToList();
        
        return models;
    }

    public async Task<List<Candle>> GetLastYearAsync(Guid instrumentId)
    {
        var from = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddYears(-1));
        var to = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        
        var entities = await context.CandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .Where(x => 
                x.Date >= from &&
                x.Date <= to)
            .OrderBy(x => x.Date)
            .AsNoTracking()
            .ToListAsync();

        if (entities.Count == 0)
            return [];
        
        var models = entities.Select(DataAccessMapper.Map).ToList();
        
        return models;
    }
}
