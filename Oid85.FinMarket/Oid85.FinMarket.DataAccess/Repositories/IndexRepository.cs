using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Logging.Services;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class IndexRepository(
    ILogService logService,
    FinMarketContext context) 
    : IIndexRepository
{
    public async Task AddAsync(List<FinIndex> indicatives)
    {
        if (indicatives is [])
            return;

        var entities = new List<FinIndexEntity>();
        
        foreach (var indicative in indicatives)
            if (!await context.IndicativeEntities
                    .AnyAsync(x => x.InstrumentId == indicative.InstrumentId))
                entities.Add(GetEntity(indicative));

        await context.IndicativeEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.IndicativeEntities
                .Where(x => x.InstrumentId == instrumentId)
                .ExecuteUpdateAsync(
                    s => s.SetProperty(
                        u => u.LastPrice, lastPrice));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            await logService.LogException(exception);
        }
    }
    
    public async Task<List<FinIndex>> GetAllAsync() =>
        (await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<FinIndex>> GetWatchListAsync() =>
        (await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<FinIndex?> GetByTickerAsync(string ticker)
    {
        var entity = await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private FinIndexEntity GetEntity(FinIndex model)
    {
        var entity = new FinIndexEntity();
        
        entity.Figi = model.Figi;
        entity.InstrumentId = model.InstrumentId;
        entity.Ticker = model.Ticker;
        entity.LastPrice = model.LastPrice;
        entity.ClassCode = model.ClassCode;
        entity.Currency = model.Currency;
        entity.InstrumentKind = model.InstrumentKind;
        entity.Name = model.Name;
        entity.Exchange = model.Exchange;
        entity.InWatchList = model.InWatchList;

        return entity;
    }
    
    private FinIndex GetModel(FinIndexEntity entity)
    {
        var model = new FinIndex();
        
        model.Id = entity.Id;
        model.Figi = entity.Figi;
        model.InstrumentId = entity.InstrumentId;
        model.Ticker = entity.Ticker;
        model.LastPrice = entity.LastPrice;
        model.ClassCode = entity.ClassCode;
        model.Currency = entity.Currency;
        model.InstrumentKind = entity.InstrumentKind;
        model.Name = entity.Name;
        model.Exchange = entity.Exchange;
        model.InWatchList = entity.InWatchList;

        return model;
    }
}