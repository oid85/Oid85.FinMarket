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

    public async Task<List<FinIndex>> GetByTickersAsync(List<string> tickers) =>
        (await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .Where(x => tickers.Contains(x.Ticker))
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

    public async Task<FinIndex?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);

        return entity is null 
            ? null 
            : GetModel(entity);
    }

    private FinIndexEntity GetEntity(FinIndex model)
    {
        var entity = new FinIndexEntity
        {
            Figi = model.Figi,
            InstrumentId = model.InstrumentId,
            Ticker = model.Ticker,
            LastPrice = model.LastPrice,
            HighTargetPrice = model.HighTargetPrice,
            LowTargetPrice = model.LowTargetPrice,
            ClassCode = model.ClassCode,
            Currency = model.Currency,
            InstrumentKind = model.InstrumentKind,
            Name = model.Name,
            Exchange = model.Exchange
        };

        return entity;
    }
    
    private FinIndex GetModel(FinIndexEntity entity)
    {
        var model = new FinIndex
        {
            Id = entity.Id,
            Figi = entity.Figi,
            InstrumentId = entity.InstrumentId,
            Ticker = entity.Ticker,
            LastPrice = entity.LastPrice,
            HighTargetPrice = entity.HighTargetPrice,
            LowTargetPrice = entity.LowTargetPrice,
            ClassCode = entity.ClassCode,
            Currency = entity.Currency,
            InstrumentKind = entity.InstrumentKind,
            Name = entity.Name,
            Exchange = entity.Exchange
        };

        return model;
    }
}