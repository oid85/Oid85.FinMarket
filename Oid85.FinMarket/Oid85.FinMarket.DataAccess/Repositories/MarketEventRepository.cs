using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Logging.Services;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class MarketEventRepository(
    ILogService logService,
    FinMarketContext context) 
    : IMarketEventRepository
{
    public async Task ActivateAsync(MarketEvent marketEvent)
    {
        if (!await context.MarketEventEntities
                .AnyAsync(x => 
                    x.InstrumentId == marketEvent.InstrumentId &&
                    x.MarketEventType == marketEvent.MarketEventType &&
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
                        x.MarketEventType == marketEvent.MarketEventType)
                    .ExecuteUpdateAsync(
                        s => s
                            .SetProperty(u => u.IsActive, true));
            
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                await logService.LogException(exception);
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
            await logService.LogException(exception);
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
    
    private MarketEventEntity GetEntity(MarketEvent model)
    {
        var entity = new MarketEventEntity
        {
            Date = model.Date,
            Time = model.Time,
            InstrumentId = model.InstrumentId,
            MarketEventType = model.MarketEventType,
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
            MarketEventType = entity.MarketEventType,
            IsActive = entity.IsActive,
            SentNotification = entity.SentNotification
        };

        return model;
    }
}