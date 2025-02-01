using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class SpreadRepository(
    ILogger logger,
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

    public async Task UpdateSpreadAsync(Spread spread)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.SpreadEntities
                .Where(x => 
                    x.FirstInstrumentId == spread.FirstInstrumentId && 
                    x.SecondInstrumentId == spread.SecondInstrumentId)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(u => u.DateTime, spread.DateTime)
                        .SetProperty(u => u.FirstInstrumentPrice, spread.FirstInstrumentPrice)
                        .SetProperty(u => u.SecondInstrumentPrice, spread.SecondInstrumentPrice)
                        .SetProperty(u => u.PriceDifference, spread.PriceDifference)
                        .SetProperty(u => u.PriceDifferencePrc, spread.PriceDifferencePrc)
                        .SetProperty(u => u.Funding, spread.Funding)
                        .SetProperty(u => u.SpreadPricePosition, spread.SpreadPricePosition));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception.Message);
        }
    }

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
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Spread?> GetByTickerAsync(string firstInstrumentTicker)
    {
        var entity = await context.SpreadEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.FirstInstrumentTicker == firstInstrumentTicker);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private SpreadEntity GetEntity(Spread model)
    {
        var entity = new SpreadEntity
        {
            DateTime = model.DateTime,
            FirstInstrumentId = model.FirstInstrumentId,
            FirstInstrumentTicker = model.FirstInstrumentTicker,
            FirstInstrumentRole = model.FirstInstrumentRole,
            FirstInstrumentPrice = model.FirstInstrumentPrice,
            SecondInstrumentId = model.SecondInstrumentId,
            SecondInstrumentTicker = model.SecondInstrumentTicker,
            SecondInstrumentRole = model.SecondInstrumentRole,
            SecondInstrumentPrice = model.SecondInstrumentPrice,
            Multiplier = model.Multiplier,
            PriceDifference = model.PriceDifference,
            PriceDifferencePrc = model.PriceDifferencePrc,
            PriceDifferenceAverage = model.PriceDifferenceAverage,
            PriceDifferenceAveragePrc = model.PriceDifferenceAveragePrc,
            Funding = model.Funding,
            SpreadPricePosition = model.SpreadPricePosition
        };

        return entity;
    }
    
    private Spread GetModel(SpreadEntity entity)
    {
        var model = new Spread
        {
            Id = entity.Id,
            DateTime = entity.DateTime,
            FirstInstrumentId = entity.FirstInstrumentId,
            FirstInstrumentTicker = entity.FirstInstrumentTicker,
            FirstInstrumentRole = entity.FirstInstrumentRole,
            FirstInstrumentPrice = entity.FirstInstrumentPrice,
            SecondInstrumentId = entity.SecondInstrumentId,
            SecondInstrumentTicker = entity.SecondInstrumentTicker,
            SecondInstrumentRole = entity.SecondInstrumentRole,
            SecondInstrumentPrice = entity.SecondInstrumentPrice,
            Multiplier = entity.Multiplier,
            PriceDifference = entity.PriceDifference,
            PriceDifferencePrc = entity.PriceDifferencePrc,
            PriceDifferenceAverage = entity.PriceDifferenceAverage,
            PriceDifferenceAveragePrc = entity.PriceDifferenceAveragePrc,
            Funding = entity.Funding,
            SpreadPricePosition = entity.SpreadPricePosition
        };

        return model;
    }
}
