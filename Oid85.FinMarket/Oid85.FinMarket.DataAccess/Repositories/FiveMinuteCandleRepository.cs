using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
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
            var entities = candles
                .Select(GetEntity);
            
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

                    SetEntity(ref entity, candle);
                }
            }

            var entities = candles
                .Select(GetEntity)
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
        .Select(GetModel)
        .OrderBy(x => x.Date.ToDateTime(x.Time))
        .ToList();

    public async Task<List<FiveMinuteCandle>> GetAsync(
        Guid instrumentId, DateTime from, DateTime to)
    {
        var candles = await GetAsync(instrumentId);
        
        var result = candles
            .Where(x => 
                x.Date.ToDateTime(x.Time) >= from && 
                x.Date.ToDateTime(x.Time) <= to)
            .ToList();

        return result;
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

        var lastCandleEntity = entitiesByMaxDate
            .OrderBy(x => x.Time)
            .Last();
        
        var candle = GetModel(lastCandleEntity);
        
        return candle;
    }
    
    private void SetEntity(ref FiveMinuteCandleEntity? entity, FiveMinuteCandle model)
    {
        entity ??= new FiveMinuteCandleEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Open = model.Open;
        entity.Close = model.Close;
        entity.High = model.High;
        entity.Low = model.Low;
        entity.Volume = model.Volume;
        entity.Date = model.Date;
        entity.Time = model.Time;
        entity.IsComplete = model.IsComplete;
    }
    
    private FiveMinuteCandleEntity GetEntity(FiveMinuteCandle model)
    {
        var entity = new FiveMinuteCandleEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Open = model.Open;
        entity.Close = model.Close;
        entity.High = model.High;
        entity.Low = model.Low;
        entity.Volume = model.Volume;
        entity.Date = model.Date;
        entity.Time = model.Time;
        entity.IsComplete = model.IsComplete;

        return entity;
    }
    
    private FiveMinuteCandle GetModel(FiveMinuteCandleEntity entity)
    {
        var model = new FiveMinuteCandle();
        
        model.Id = entity.Id;
        model.InstrumentId = entity.InstrumentId;
        model.Open = entity.Open;
        model.Close = entity.Close;
        model.High = entity.High;
        model.Low = entity.Low;
        model.Volume = entity.Volume;
        model.Date = entity.Date;
        model.Time = entity.Time;
        model.IsComplete = entity.IsComplete;

        return model;
    }
}