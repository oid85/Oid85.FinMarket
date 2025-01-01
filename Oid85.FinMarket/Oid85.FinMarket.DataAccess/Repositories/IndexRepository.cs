using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class IndexRepository(
    FinMarketContext context) : IIndexRepository
{
    public async Task AddOrUpdateAsync(List<FinIndex> indicatives)
    {
        if (indicatives.Count == 0)
            return;
        
        foreach (var indicative in indicatives)
        {
            var entity = context.IndicativeEntities
                .FirstOrDefault(x => 
                    x.InstrumentId == indicative.InstrumentId);

            if (entity is null)
            {
                SetEntity(ref entity, indicative);
                
                if (entity is not null)
                    await context.IndicativeEntities.AddAsync(entity);
            }

            else
            {
                SetEntity(ref entity, indicative);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<FinIndex>> GetAllAsync() =>
        (await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<FinIndex>> GetWatchListAsync() =>
        (await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<FinIndex?> GetByTickerAsync(string ticker)
    {
        var entity = await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private void SetEntity(ref FinIndexEntity? entity, FinIndex model)
    {
        entity ??= new FinIndexEntity();
        
        entity.Figi = model.Figi;
        entity.InstrumentId = model.InstrumentId;
        entity.Ticker = model.Ticker;
        entity.Price = model.Price;
        entity.ClassCode = model.ClassCode;
        entity.Currency = model.Currency;
        entity.InstrumentKind = model.InstrumentKind;
        entity.Name = model.Name;
        entity.Exchange = model.Exchange;
        entity.InWatchList = model.InWatchList;
    }
    
    private FinIndex GetModel(FinIndexEntity entity)
    {
        var model = new FinIndex();
        
        model.Id = entity.Id;
        model.Figi = entity.Figi;
        model.InstrumentId = entity.InstrumentId;
        model.Ticker = entity.Ticker;
        model.Price = entity.Price;
        model.ClassCode = entity.ClassCode;
        model.Currency = entity.Currency;
        model.InstrumentKind = entity.InstrumentKind;
        model.Name = entity.Name;
        model.Exchange = entity.Exchange;
        model.InWatchList = entity.InWatchList;

        return model;
    }
}