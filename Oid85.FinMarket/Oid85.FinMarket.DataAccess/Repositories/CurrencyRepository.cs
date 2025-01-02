using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class CurrencyRepository(
    FinMarketContext context) 
    : ICurrencyRepository
{
    public async Task AddOrUpdateAsync(List<Currency> currencies)
    {
        if (currencies.Count == 0)
            return;
        
        foreach (var currency in currencies)
        {
            var entity = context.CurrencyEntities
                .FirstOrDefault(x => 
                    x.InstrumentId == currency.InstrumentId);

            if (entity is null)
            {
                SetEntity(ref entity, currency);
                
                if (entity is not null)
                    await context.CurrencyEntities.AddAsync(entity);
            }

            else
            {
                SetEntity(ref entity, currency);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<Currency>> GetAllAsync() =>
        (await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<Currency>> GetWatchListAsync() =>
        (await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Currency?> GetByTickerAsync(string ticker)
    {
        var entity = await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        return entity is null 
            ? null 
            : GetModel(entity);
    }

    public async Task<Currency?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);

        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private void SetEntity(ref CurrencyEntity? entity, Currency model)
    {
        entity ??= new CurrencyEntity();
        
        entity.Ticker = model.Ticker;
        entity.Price = model.Price;
        entity.Isin = model.Isin;
        entity.Figi = model.Figi;
        entity.ClassCode = model.ClassCode;
        entity.Name = model.Name;
        entity.IsoCurrencyName = model.IsoCurrencyName;
        entity.InstrumentId = model.InstrumentId;
        entity.InWatchList = model.InWatchList;
    }
    
    private Currency GetModel(CurrencyEntity entity)
    {
        var model = new Currency();
        
        model.Id = entity.Id;
        model.Ticker = entity.Ticker;
        model.Price = entity.Price;
        model.Isin = entity.Isin;
        model.Figi = entity.Figi;
        model.ClassCode = entity.ClassCode;
        model.Name = entity.Name;
        model.IsoCurrencyName = entity.IsoCurrencyName;
        model.InstrumentId = entity.InstrumentId;
        model.InWatchList = entity.InWatchList;

        return model;
    }
}