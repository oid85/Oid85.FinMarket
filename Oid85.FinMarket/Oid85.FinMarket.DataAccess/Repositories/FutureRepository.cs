﻿using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class FutureRepository(
    FinMarketContext context) 
    : IFutureRepository
{
    public async Task AddAsync(List<Future> futures)
    {
        if (futures is [])
            return;

        var entities = new List<FutureEntity>();
        
        foreach (var future in futures)
            if (!await context.FutureEntities
                    .AnyAsync(x => x.InstrumentId == future.InstrumentId))
                entities.Add(GetEntity(future));

        await context.FutureEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
    
    public Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice) =>
        context.FutureEntities
            .Where(x => x.InstrumentId == instrumentId)
            .ExecuteUpdateAsync(
                s => s.SetProperty(
                    u => u.LastPrice, lastPrice));

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
    
    private FutureEntity GetEntity(Future model)
    {
        var entity = new FutureEntity();
        
        entity.Ticker = model.Ticker;
        entity.LastPrice = model.LastPrice;
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

        return entity;
    }
    
    private Future GetModel(FutureEntity entity)
    {
        var model = new Future();
        
        model.Id = entity.Id;
        model.Ticker = entity.Ticker;
        model.LastPrice = entity.LastPrice;
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