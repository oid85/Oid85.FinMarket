using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class FutureRepository(
    FinMarketContext context) 
    : IFutureRepository
{
    public async Task AddOrUpdateAsync(List<Future> futures)
    {
        if (futures.Count == 0)
            return;
        
        foreach (var future in futures)
        {
            var entity = context.FutureEntities
                .FirstOrDefault(x => 
                    x.InstrumentId == future.InstrumentId);

            if (entity is null)
            {
                SetEntity(ref entity, future);
                
                if (entity is not null)
                    await context.FutureEntities.AddAsync(entity);
            }

            else
            {
                SetEntity(ref entity, future);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<Future>> GetAllAsync() =>
        (await context.FutureEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<Future>> GetWatchListAsync() =>
        (await context.FutureEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Future?> GetByTickerAsync(string ticker)
    {
        var entity = await context.FutureEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    public async Task<Future?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.FutureEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);

        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private void SetEntity(ref FutureEntity? entity, Future model)
    {
        entity ??= new FutureEntity();
        
        entity.Id = model.Id;
        entity.Ticker = model.Ticker;
        entity.Price = model.Price;
        entity.Figi = model.Figi;
        entity.InstrumentId = model.InstrumentId;
        entity.Name = model.Name;
        entity.ExpirationDate = model.ExpirationDate;
        entity.InWatchList = model.InWatchList;
        entity.Lot = model.Lot;
        entity.FirstTradeDate = model.FirstTradeDate;
        entity.LastTradeDate = model.LastTradeDate;
        entity.FutureType = model.FutureType;
        entity.AssetType = model.AssetType;
        entity.BasicAsset = model.BasicAsset;
        entity.BasicAssetSize = model.BasicAssetSize;
        entity.InitialMarginOnBuy = model.InitialMarginOnBuy;
        entity.InitialMarginOnSell = model.InitialMarginOnSell;
        entity.MinPriceIncrementAmount = model.MinPriceIncrementAmount;
    }
    
    private Future GetModel(FutureEntity entity)
    {
        var model = new Future();
        
        model.Id = entity.Id;
        model.Ticker = entity.Ticker;
        model.Price = entity.Price;
        model.Figi = entity.Figi;
        model.InstrumentId = entity.InstrumentId;
        model.Name = entity.Name;
        model.ExpirationDate = entity.ExpirationDate;
        model.InWatchList = entity.InWatchList;
        model.Lot = entity.Lot;
        model.FirstTradeDate = entity.FirstTradeDate;
        model.LastTradeDate = entity.LastTradeDate;
        model.FutureType = entity.FutureType;
        model.AssetType = entity.AssetType;
        model.BasicAsset = entity.BasicAsset;
        model.BasicAssetSize = entity.BasicAssetSize;
        model.InitialMarginOnBuy = entity.InitialMarginOnBuy;
        model.InitialMarginOnSell = entity.InitialMarginOnSell;
        model.MinPriceIncrementAmount = entity.MinPriceIncrementAmount;

        return model;
    }
}