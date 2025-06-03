using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class CandleRepository(
    FinMarketContext context) 
    : ICandleRepository
{
    public async Task AddOrUpdateAsync(List<Candle> candles)
    {
        var completedCandles = candles
            .Where(x => x.IsComplete).ToList();
        
        if (completedCandles is [])
            return;

        var entities = new List<CandleEntity>();
        
        foreach (var candle in completedCandles)
            if (!await context.CandleEntities
                    .AnyAsync(x => 
                        x.InstrumentId == candle.InstrumentId
                        && x.Date == candle.Date))
                entities.Add(DataAccessMapper.Map(candle));

        await context.CandleEntities.AddRangeAsync(entities);
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

    public async Task<List<Candle>> GetLastYearAsync(Guid instrumentId) =>
        await GetLastYearsAsync(instrumentId, 1);

    public async Task<List<Candle>> GetLastTwoYearsAsync(Guid instrumentId) =>
        await GetLastYearsAsync(instrumentId, 2);

    private async Task<List<Candle>> GetLastYearsAsync(Guid instrumentId, int years)
    {
        var from = DateOnly.FromDateTime(DateTime.Today.AddYears(-1 * years));
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

    public async Task<List<Candle>> GetAsync(Guid instrumentId, DateOnly from, DateOnly to)
    {
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

    public async Task<List<Candle>> GetAsync(string ticker, DateOnly from, DateOnly to)
    {
        var instrumentEntity = await context.InstrumentEntities
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        if (instrumentEntity is null)
            return [];
        
        var entities = await context.CandleEntities
            .Where(x => x.InstrumentId == instrumentEntity.InstrumentId)
            .Where(x => 
                x.Date >= from &&
                x.Date <= to)
            .OrderBy(x => x.Date)
            .AsNoTracking()
            .ToListAsync();

        return entities.Count == 0 ? [] : entities.Select(DataAccessMapper.Map).ToList();
    }
}
