using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Logging.Services;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class CurrencyRepository(
    ILogService logService,
    FinMarketContext context) 
    : ICurrencyRepository
{
    public async Task AddAsync(List<Currency> currencies)
    {
        if (currencies is [])
            return;

        var entities = new List<CurrencyEntity>();
        
        foreach (var currency in currencies)
            if (!await context.CurrencyEntities
                    .AnyAsync(x => x.InstrumentId == currency.InstrumentId))
                entities.Add(GetEntity(currency));

        await context.CurrencyEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.CurrencyEntities
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
    
    public async Task<List<Currency>> GetAllAsync() =>
        (await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<Currency>> GetWatchListAsync() =>
        (await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Currency?> GetByTickerAsync(string ticker)
    {
        var entity = await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        return entity is null 
            ? null 
            : GetModel(entity);
    }

    public async Task<Currency?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);

        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private CurrencyEntity GetEntity(Currency model)
    {
        var entity = new CurrencyEntity
        {
            Ticker = model.Ticker,
            LastPrice = model.LastPrice,
            HighTargetPrice = model.HighTargetPrice,
            LowTargetPrice = model.LowTargetPrice,
            Isin = model.Isin,
            Figi = model.Figi,
            ClassCode = model.ClassCode,
            Name = model.Name,
            IsoCurrencyName = model.IsoCurrencyName,
            InstrumentId = model.InstrumentId,
            InWatchList = model.InWatchList
        };

        return entity;
    }
    
    private Currency GetModel(CurrencyEntity entity)
    {
        var model = new Currency
        {
            Id = entity.Id,
            Ticker = entity.Ticker,
            LastPrice = entity.LastPrice,
            HighTargetPrice = entity.HighTargetPrice,
            LowTargetPrice = entity.LowTargetPrice,
            Isin = entity.Isin,
            Figi = entity.Figi,
            ClassCode = entity.ClassCode,
            Name = entity.Name,
            IsoCurrencyName = entity.IsoCurrencyName,
            InstrumentId = entity.InstrumentId,
            InWatchList = entity.InWatchList
        };

        return model;
    }
}