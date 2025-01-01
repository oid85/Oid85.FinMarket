using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class SpreadRepository(
    FinMarketContext context) 
    : ISpreadRepository
{
    public async Task AddOrUpdateAsync(List<Spread> spreads)
    {
        if (spreads.Count == 0)
            return;
        
        foreach (var spread in spreads)
        {
            var entity = context.SpreadEntities
                .FirstOrDefault(x => 
                    x.FirstInstrumentId == spread.FirstInstrumentId);

            if (entity is null)
            {
                SetEntity(ref entity, spread);
                
                if (entity is not null)
                    await context.SpreadEntities.AddAsync(entity);
            }

            else
            {
                SetEntity(ref entity, spread);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<Spread>> GetAllAsync() =>
        (await context.SpreadEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.FirstInstrumentTicker)
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<Spread>> GetWatchListAsync() =>
        (await context.SpreadEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.FirstInstrumentTicker)
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Spread?> GetByTickerAsync(string firstInstrumentTicker)
    {
        var entity = await context.SpreadEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.FirstInstrumentTicker == firstInstrumentTicker);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private void SetEntity(ref SpreadEntity? entity, Spread model)
    {
        entity ??= new SpreadEntity();
        
        entity.DateTime = model.DateTime;
        entity.FirstInstrumentId = model.FirstInstrumentId;
        entity.FirstInstrumentTicker = model.FirstInstrumentTicker;
        entity.FirstInstrumentRole = model.FirstInstrumentRole;
        entity.FirstInstrumentPrice = model.FirstInstrumentPrice;
        entity.SecondInstrumentId = model.SecondInstrumentId;
        entity.SecondInstrumentTicker = model.SecondInstrumentTicker;
        entity.SecondInstrumentRole = model.SecondInstrumentRole;
        entity.SecondInstrumentPrice = model.SecondInstrumentPrice;
        entity.PriceDifference = model.PriceDifference;
        entity.PriceDifferencePrc = model.PriceDifferencePrc;
        entity.Funding = model.Funding;
        entity.SpreadPricePosition = model.SpreadPricePosition;
        entity.InWatchList = model.InWatchList;
    }
    
    private Spread GetModel(SpreadEntity entity)
    {
        var model = new Spread();
        
        model.Id = entity.Id;
        model.DateTime = entity.DateTime;
        model.FirstInstrumentId = entity.FirstInstrumentId;
        model.FirstInstrumentTicker = entity.FirstInstrumentTicker;
        model.FirstInstrumentRole = entity.FirstInstrumentRole;
        model.FirstInstrumentPrice = entity.FirstInstrumentPrice;
        model.SecondInstrumentId = entity.SecondInstrumentId;
        model.SecondInstrumentTicker = entity.SecondInstrumentTicker;
        model.SecondInstrumentRole = entity.SecondInstrumentRole;
        model.SecondInstrumentPrice = entity.SecondInstrumentPrice;
        model.PriceDifference = entity.PriceDifference;
        model.PriceDifferencePrc = entity.PriceDifferencePrc;
        model.Funding = entity.Funding;
        model.SpreadPricePosition = entity.SpreadPricePosition;
        model.InWatchList = entity.InWatchList;

        return model;
    }
}
