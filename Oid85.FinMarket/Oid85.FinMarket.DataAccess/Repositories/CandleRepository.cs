using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class CandleRepository(
    FinMarketContext context,
    IMapper mapper) : ICandleRepository
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
                .Select(x => mapper.Map<CandleEntity>(x));
            await context.CandleEntities.AddRangeAsync(entities);
        }
        
        else
        {
            if (!lastEntity.IsComplete)
            {
                var candle = candles.First(x => x.Date == lastEntity.Date);
                
                lastEntity.Open = candle.Open;
                lastEntity.Close = candle.Close;
                lastEntity.High = candle.High;
                lastEntity.Low = candle.Low;
                lastEntity.Volume = candle.Volume;
                lastEntity.IsComplete = candle.IsComplete;
            }

            var entities = candles
                .Select(x => mapper.Map<CandleEntity>(x))
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
            .Select(x => mapper.Map<Candle>(x))
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
