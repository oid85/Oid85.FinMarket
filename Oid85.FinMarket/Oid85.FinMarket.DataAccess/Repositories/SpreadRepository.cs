using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class SpreadRepository(
    FinMarketContext context) 
    : ISpreadRepository
{
    public async Task AddAsync(List<Spread> spreads)
    {
        if (spreads is [])
            return;

        var entities = new List<SpreadEntity>();
        
        foreach (var spread in spreads)
            if (!await context.SpreadEntities
                    .AnyAsync(x => 
                        x.FirstInstrumentId == spread.FirstInstrumentId &&
                        x.SecondInstrumentId == spread.SecondInstrumentId))
                entities.Add(GetEntity(spread));

        await context.SpreadEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public Task UpdateSpreadAsync(Spread spread) =>
        context.SpreadEntities
            .Where(x => 
                x.FirstInstrumentId == spread.FirstInstrumentId && 
                x.SecondInstrumentId == spread.SecondInstrumentId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(u => u.DateTime, spread.DateTime)
                    .SetProperty(u => u.FirstInstrumentPrice, spread.FirstInstrumentPrice)
                    .SetProperty(u => u.SecondInstrumentPrice, spread.SecondInstrumentPrice)
                    .SetProperty(u => u.PriceDifference, spread.PriceDifference)
                    .SetProperty(u => u.Funding, spread.Funding)
                    .SetProperty(u => u.SpreadPricePosition, spread.SpreadPricePosition));

    public Task SetAsDeletedAsync(Spread spread) =>
        context.SpreadEntities
            .Where(x => 
                x.FirstInstrumentId == spread.FirstInstrumentId && 
                x.SecondInstrumentId == spread.SecondInstrumentId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(u => u.IsDeleted, true)
                    .SetProperty(u => u.DeletedAt, DateTime.UtcNow));

    public async Task<List<Spread>> GetAllAsync() =>
        (await context.SpreadEntities
            .Where(x => !x.IsDeleted)
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
    
    private SpreadEntity GetEntity(Spread model)
    {
        var entity = new SpreadEntity();
        
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
        entity.PriceDifferenceAverage = model.PriceDifferenceAverage;
        entity.PriceDifferenceAveragePrc = model.PriceDifferenceAveragePrc;
        entity.Funding = model.Funding;
        entity.SpreadPricePosition = model.SpreadPricePosition;

        return entity;
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
        model.PriceDifferenceAverage = entity.PriceDifferenceAverage;
        model.PriceDifferenceAveragePrc = entity.PriceDifferenceAveragePrc;
        model.Funding = entity.Funding;
        model.SpreadPricePosition = entity.SpreadPricePosition;

        return model;
    }
}
