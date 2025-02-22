using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
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
                    x.MarketEventType == marketEvent.MarketEventType &&
                    x.MarketEventText == marketEvent.MarketEventText)) 
            return;
        
        await context.AddAsync(GetEntity(marketEvent));
        await context.SaveChangesAsync();
    }

    public Task ActivateAsync(MarketEvent marketEvent) =>
        SetIsActiveFlagAsync(marketEvent, true);

    public Task DeactivateAsync(MarketEvent marketEvent) =>
        SetIsActiveFlagAsync(marketEvent, false);

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
        .Select(GetModel)
        .ToList();

    private async Task SetSentNotificationFlagAsync(MarketEvent marketEvent, bool value)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.MarketEventEntities
                .Where(x => x.Id == marketEvent.Id)
                .ExecuteUpdateAsync(
                    s => s.SetProperty(
                        entity => entity.SentNotification, value));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }

    private async Task SetIsActiveFlagAsync(MarketEvent marketEvent, bool value)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.MarketEventEntities
                .Where(x => 
                    x.InstrumentId == marketEvent.InstrumentId &&
                    x.MarketEventType == marketEvent.MarketEventType &&
                    x.MarketEventText == marketEvent.MarketEventText)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(entity => entity.IsActive, value));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }
    
    private MarketEventEntity GetEntity(MarketEvent model)
    {
        var entity = new MarketEventEntity
        {
            Date = model.Date,
            Time = model.Time,
            InstrumentId = model.InstrumentId,
            Ticker = model.Ticker,
            InstrumentName = model.InstrumentName,
            MarketEventType = model.MarketEventType,
            MarketEventText = model.MarketEventText,
            IsActive = model.IsActive,
            SentNotification = model.SentNotification
        };

        return entity;
    }
    
    private MarketEvent GetModel(MarketEventEntity entity)
    {
        var model = new MarketEvent
        {
            Id = entity.Id,
            Date = entity.Date,
            Time = entity.Time,
            InstrumentId = entity.InstrumentId,
            Ticker = entity.Ticker,
            InstrumentName = entity.InstrumentName,
            MarketEventType = entity.MarketEventType,
            MarketEventText = entity.MarketEventText,
            IsActive = entity.IsActive,
            SentNotification = entity.SentNotification
        };

        return model;
    }
}