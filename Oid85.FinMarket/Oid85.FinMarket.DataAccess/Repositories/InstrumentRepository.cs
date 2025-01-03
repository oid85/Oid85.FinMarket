using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class InstrumentRepository(
    FinMarketContext context) 
    : IInstrumentRepository
{
    public async Task AddOrUpdateAsync(List<Instrument> tickers)
    {
        if (tickers.Count == 0)
            return;
        
        foreach (var ticker in tickers)
        {
            var entity = context.InstrumentEntities
                .FirstOrDefault(x => 
                    x.InstrumentId == ticker.InstrumentId);

            if (entity is null)
            {
                SetEntity(ref entity, ticker);
                
                if (entity is not null)
                    await context.InstrumentEntities.AddAsync(entity);
            }

            else
            {
                SetEntity(ref entity, ticker);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<Instrument>> GetAllAsync() =>
        (await context.InstrumentEntities
            .OrderBy(x => x.Name)
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Instrument?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.InstrumentEntities
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    public async Task<Instrument?> GetByNameAsync(string name)
    {
        var entity = await context.InstrumentEntities
            .FirstOrDefaultAsync(x => x.Name == name);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private void SetEntity(ref InstrumentEntity? entity, Instrument model)
    {
        entity ??= new InstrumentEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Ticker = model.Ticker;
        entity.Name = model.Name;
        entity.Type = model.Type;
    }
    
    private Instrument GetModel(InstrumentEntity entity)
    {
        var model = new Instrument();
        
        model.Id = entity.Id;
        model.InstrumentId = entity.InstrumentId;
        model.Ticker = entity.Ticker;
        model.Name = entity.Name;
        model.Type = entity.Type;

        return model;
    }
}