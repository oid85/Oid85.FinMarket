using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class HourlyCandleRepository(
    FinMarketContext context) 
    : IHourlyCandleRepository
{
    public async Task AddOrUpdateAsync(List<HourlyCandle> candles)
    {
        var completedCandles = candles
            .Where(x => x.IsComplete).ToList();
        
        if (completedCandles is [])
            return;

        var entities = new List<HourlyCandleEntity>();
        
        foreach (var candle in completedCandles)
            if (!await context.HourlyCandleEntities
                    .AnyAsync(x => 
                        x.InstrumentId == candle.InstrumentId && 
                        x.Date == candle.Date &&
                        x.Time == candle.Time))
                entities.Add(DataAccessMapper.Map(candle));

        await context.HourlyCandleEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task<HourlyCandle?> GetLastAsync(Guid instrumentId)
    {
        var entity = await context.HourlyCandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .OrderByDescending(x => x.Date)
            .Take(1)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return entity is null ? null : DataAccessMapper.Map(entity);
    }
}