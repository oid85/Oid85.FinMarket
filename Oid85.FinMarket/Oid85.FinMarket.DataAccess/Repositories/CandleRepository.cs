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
        var entity = new CandleEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Open = model.Open;
        entity.Close = model.Close;
        entity.High = model.High;
        entity.Low = model.Low;
        entity.Volume = model.Volume;
        entity.Date = model.Date;
        entity.IsComplete = model.IsComplete;

        return entity;
    }
    
    private Candle GetModel(CandleEntity entity)
    {
        var model = new Candle();
        
        model.Id = entity.Id;
        model.InstrumentId = entity.InstrumentId;
        model.Open = entity.Open;
        model.Close = entity.Close;
        model.High = entity.High;
        model.Low = entity.Low;
        model.Volume = entity.Volume;
        model.Date = entity.Date;
        model.IsComplete = entity.IsComplete;

        return model;
    }
}
