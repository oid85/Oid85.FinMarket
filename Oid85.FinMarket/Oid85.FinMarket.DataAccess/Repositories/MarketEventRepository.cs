using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class MarketEventRepository(
    ILogger logger,
    FinMarketContext context) 
    : IMarketEventRepository
{
    public async Task AddIfNotExistsAsync(MarketEvent marketEvent)
    {
        if (await context.MarketEventEntities
                .AnyAsync(x =>
                    x.InstrumentId == marketEvent.InstrumentId &&
                    x.MarketEventType == marketEvent.MarketEventType)) 
            return;
        
        await context.AddAsync(DataAccessMapper.Map(marketEvent));
        await context.SaveChangesAsync();
    }

    public Task ActivateAsync(MarketEvent marketEvent) =>
        SetIsActiveFlagTrueAsync(marketEvent);

    public Task DeactivateAsync(MarketEvent marketEvent) =>
        SetIsActiveFlagFalseAsync(marketEvent);

    public Task MarkAsSentAsync(MarketEvent marketEvent) =>
        SetSentNotificationFlagAsync(marketEvent, true);
    
    public Task MarkAsNoSentAsync(MarketEvent marketEvent) =>
        SetSentNotificationFlagAsync(marketEvent, false);
    
    public async Task<List<MarketEvent>> GetActivatedAsync() =>
        (await context.MarketEventEntities
            .Where(x => x.IsActive)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();

    private async Task SetSentNotificationFlagAsync(MarketEvent marketEvent, bool value)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.MarketEventEntities
                .Where(x => x.Id == marketEvent.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(entity => entity.SentNotification, value));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }

    private async Task SetIsActiveFlagTrueAsync(MarketEvent marketEvent)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.MarketEventEntities
                .Where(x => 
                    x.InstrumentId == marketEvent.InstrumentId &&
                    x.MarketEventType == marketEvent.MarketEventType &&
                    x.IsActive == false)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(entity => entity.IsActive, true)
                    .SetProperty(entity => entity.Date, DateOnly.FromDateTime(DateTime.UtcNow))
                    .SetProperty(entity => entity.Time, TimeOnly.FromDateTime(DateTime.UtcNow)));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }
    
    private async Task SetIsActiveFlagFalseAsync(MarketEvent marketEvent)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.MarketEventEntities
                .Where(x => 
                    x.InstrumentId == marketEvent.InstrumentId &&
                    x.MarketEventType == marketEvent.MarketEventType &&
                    x.IsActive == true)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(entity => entity.IsActive, false)
                    .SetProperty(entity => entity.Date, DateOnly.FromDateTime(DateTime.UtcNow))
                    .SetProperty(entity => entity.Time, TimeOnly.FromDateTime(DateTime.UtcNow)));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }
}