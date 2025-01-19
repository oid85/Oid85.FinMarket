using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
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
            var entities = candles
                .Select(GetEntity);
            
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

                    SetEntity(ref entity, candle);
                }
            }

            var entities = candles
                .Select(GetEntity)
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
        .Select(GetModel)
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
        
        var model = GetModel(entity);
        
        return model;
    }

    public async Task<List<Candle>> GetTwoLastAsync(Guid instrumentId)
    {
        var entities = await context.CandleEntities
            .Where(x => x.InstrumentId == instrumentId)
            .OrderByDescending(x => x.Date)
            .Take(2)
            .AsNoTracking()
            .ToListAsync();

        if (entities.Count < 2)
            return [];
        
        var models = entities
            .Select(GetModel)
            .ToList();
        
        return models;
    }

    private void SetEntity(ref CandleEntity? entity, Candle model)
    {
        entity ??= new CandleEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Open = model.Open;
        entity.Close = model.Close;
        entity.High = model.High;
        entity.Low = model.Low;
        entity.Volume = model.Volume;
        entity.Date = model.Date;
        entity.IsComplete = model.IsComplete;
    }
    
    private CandleEntity GetEntity(Candle model)
    {
        var entity = new CandleEntity
        {
            InstrumentId = model.InstrumentId,
            Open = model.Open,
            Close = model.Close,
            High = model.High,
            Low = model.Low,
            Volume = model.Volume,
            Date = model.Date,
            IsComplete = model.IsComplete
        };

        return entity;
    }
    
    private Candle GetModel(CandleEntity entity)
    {
        var model = new Candle
        {
            Id = entity.Id,
            InstrumentId = entity.InstrumentId,
            Open = entity.Open,
            Close = entity.Close,
            High = entity.High,
            Low = entity.Low,
            Volume = entity.Volume,
            Date = entity.Date,
            IsComplete = entity.IsComplete
        };

        return model;
    }
}
