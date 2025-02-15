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
    public async Task ActivateAsync(MarketEvent marketEvent)
    {
        if (!await context.MarketEventEntities
                .AnyAsync(x => 
                    x.InstrumentId == marketEvent.InstrumentId &&
                    x.MarketEventType == marketEvent.MarketEventType &&
                    x.MarketEventText == marketEvent.MarketEventText &&
                    x.IsActive))
            await context.AddAsync(GetEntity(marketEvent));

        else
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
                            .SetProperty(u => u.IsActive, true));
            
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                logger.Error(exception.Message);
            }
        }
        
        await context.SaveChangesAsync();
    }

    public async Task DeactivateAsync(MarketEvent marketEvent)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.MarketEventEntities
                .Where(x => 
                    x.InstrumentId == marketEvent.InstrumentId &&
                    x.MarketEventType == marketEvent.MarketEventType)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(u => u.IsActive, false));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception.Message);
        }
    }

    public async Task<List<MarketEvent>> GetActivatedAsync() =>
        (await context.MarketEventEntities
            .Where(x => x.IsActive)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task SetSentNotificationAsync(Guid marketEventId)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.MarketEventEntities
                .Where(x => x.Id == marketEventId)
                .ExecuteUpdateAsync(
                    s => s.SetProperty(
                        u => u.SentNotification, true));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception.Message);
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
            MarketEventType = entity.MarketEventType,
            MarketEventText = entity.MarketEventText,
            IsActive = entity.IsActive,
            SentNotification = entity.SentNotification
        };

        return model;
    }
}