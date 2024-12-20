using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class CandleRepository(
    FinMarketContext context) : ICandleRepository
{
    public async Task AddOrUpdateAsync(List<Candle> candles)
    {
        if (!candles.Any())
            return;
        
        var lastEntity = await GetLastAsync(
            candles.First().Ticker, candles.First().Timeframe);

        if (lastEntity is null)
        {
            var entities = candles
                .Select(x => x.Adapt<CandleEntity>());
            await context.CandleEntities.AddRangeAsync(entities);
        }
        
        else
        {
            if (!lastEntity.IsComplete)
            {
                var candle = candles.First(x => x.Date == lastEntity.Date);
                lastEntity.Adapt(candle);
            }

            var entities = candles
                .Select(x => x.Adapt<CandleEntity>())
                .Where(x => x.Date > lastEntity.Date);
                
            await context.CandleEntities.AddRangeAsync(entities);  
        }

        await context.SaveChangesAsync();
    }

    public Task<List<Candle>> GetAsync(string ticker, string timeframe) =>
        context.CandleEntities
            .Where(x => ticker == x.Ticker)
            .Where(x => x.Timeframe == timeframe)
            .OrderBy(x => x.Date)
            .Select(x => x.Adapt<Candle>())
            .ToListAsync();

    private async Task<CandleEntity?> GetLastAsync(string ticker, string timeframe)
    {
        bool exists = await context.CandleEntities
            .Where(x => x.Timeframe == timeframe)
            .Where(x => x.Ticker == ticker)
            .AnyAsync();

        if (!exists)
            return null;
        
        var maxDate = await context.CandleEntities
            .Where(x => x.Timeframe == timeframe)
            .Where(x => x.Ticker == ticker)
            .MaxAsync(x => x.Date);

        var entity = await context.CandleEntities
            .Where(x => x.Timeframe == timeframe)
            .Where(x => x.Ticker == ticker)
            .FirstAsync(x => x.Date == maxDate);
        
        return entity;
    }
}
