using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class DailyCandleRepository(
    IDbContextFactory<FinMarketContext> contextFactory)
    : IDailyCandleRepository
{
    public async Task AddOrUpdateAsync(List<DailyCandle> candles)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        // Удалим незавершенные свечи
        var instrumentIds = candles.Select(x => x.InstrumentId).Distinct();

        foreach (var instrumentId in instrumentIds) 
            await context.DailyCandleEntities
                .Where(x => x.InstrumentId == instrumentId && x.IsComplete)
                .ExecuteDeleteAsync();
        
        await context.SaveChangesAsync();
        
        if (candles is [])
            return;

        var entities = new List<DailyCandleEntity>();
        
        foreach (var candle in candles)
            if (!await context.DailyCandleEntities
                    .AnyAsync(x => 
                        x.InstrumentId == candle.InstrumentId
                        && x.Date == candle.Date))
                entities.Add(DataAccessMapper.Map(candle));

        await context.DailyCandleEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
    
    public async Task<DailyCandle?> GetLastAsync(Guid instrumentId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var entity = await context.DailyCandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .OrderByDescending(x => x.Date)
            .Take(1)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return entity is null ? null : DataAccessMapper.Map(entity);
    }

    public async Task<List<DailyCandle>> GetLastYearAsync(Guid instrumentId) =>
        await GetLastYearsAsync(instrumentId, 1);

    public async Task<List<DailyCandle>> GetLastTwoYearsAsync(Guid instrumentId) =>
        await GetLastYearsAsync(instrumentId, 2);

    private async Task<List<DailyCandle>> GetLastYearsAsync(Guid instrumentId, int years)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var from = DateOnly.FromDateTime(DateTime.Today.AddYears(-1 * years));
        var to = DateOnly.FromDateTime(DateTime.Today);
        
        var entities = await context.DailyCandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .Where(x => 
                x.Date >= from &&
                x.Date <= to)
            .OrderBy(x => x.Date)
            .AsNoTracking()
            .ToListAsync();

        return entities.Count == 0 ? [] : entities.Select(DataAccessMapper.Map).ToList();
    }

    public async Task<DailyCandle?> GetAsync(Guid instrumentId, DateOnly date)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var entity = await context.DailyCandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .Where(x => x.Date == date)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return entity is null ? null : DataAccessMapper.Map(entity);
    }

    public async Task<List<DailyCandle>> GetAsync(Guid instrumentId, DateOnly from, DateOnly to)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var entities = await context.DailyCandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .Where(x => 
                x.Date >= from &&
                x.Date <= to)
            .OrderBy(x => x.Date)
            .AsNoTracking()
            .ToListAsync();

        return entities.Count == 0 ? [] : entities.Select(DataAccessMapper.Map).ToList();
    }

    public async Task<List<DailyCandle>> GetAsync(string ticker, DateOnly from, DateOnly to)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var instrumentEntity = await context.InstrumentEntities
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        if (instrumentEntity is null)
            return [];
        
        var entities = await context.DailyCandleEntities
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
