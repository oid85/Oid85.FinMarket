using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
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
                entities.Add(DataAccessMapper.Map(spread));

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
                .ExecuteUpdateAsync(x => x
                        .SetProperty(entity => entity.DateTime, spread.DateTime)
                        .SetProperty(entity => entity.FirstInstrumentPrice, spread.FirstInstrumentPrice)
                        .SetProperty(entity => entity.SecondInstrumentPrice, spread.SecondInstrumentPrice)
                        .SetProperty(entity => entity.PriceDifference, spread.PriceDifference)
                        .SetProperty(entity => entity.PriceDifferencePrc, spread.PriceDifferencePrc)
                        .SetProperty(entity => entity.Funding, spread.Funding)
                        .SetProperty(entity => entity.SpreadPricePosition, spread.SpreadPricePosition));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }

    public async Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.SpreadEntities
                .Where(x => 
                    x.FirstInstrumentId == instrumentId)
                .ExecuteUpdateAsync(x => x
                        .SetProperty(entity => entity.FirstInstrumentPrice, lastPrice));
            
            await context.SpreadEntities
                .Where(x => 
                    x.SecondInstrumentId == instrumentId)
                .ExecuteUpdateAsync(x => x
                        .SetProperty(entity => entity.SecondInstrumentPrice, lastPrice));            
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }

    public Task SetAsDeletedAsync(Spread spread) =>
        context.SpreadEntities
            .Where(x => 
                x.FirstInstrumentId == spread.FirstInstrumentId && 
                x.SecondInstrumentId == spread.SecondInstrumentId)
            .ExecuteUpdateAsync(x => x
                    .SetProperty(entity => entity.IsDeleted, true)
                    .SetProperty(entity => entity.DeletedAt, DateTime.UtcNow));

    public async Task<List<Spread>> GetAllAsync() =>
        (await context.SpreadEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.FirstInstrumentTicker)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();
}
