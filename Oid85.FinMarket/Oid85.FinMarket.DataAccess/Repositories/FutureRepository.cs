using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Logging.Services;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class FutureRepository(
    ILogService logService,
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
    
    public async Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.FutureEntities
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

    public async Task<List<Future>> GetAllAsync() =>
        (await context.FutureEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<Future>> GetWatchListAsync() =>
        (await context.FutureEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Future?> GetByTickerAsync(string ticker)
    {
        var entity = await context.FutureEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    public async Task<Future?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.FutureEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);

        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private FutureEntity GetEntity(Future model)
    {
        var entity = new FutureEntity
        {
            Ticker = model.Ticker,
            LastPrice = model.LastPrice,
            HighTargetPrice = model.HighTargetPrice,
            LowTargetPrice = model.LowTargetPrice,
            Figi = model.Figi,
            InstrumentId = model.InstrumentId,
            Name = model.Name,
            ExpirationDate = model.ExpirationDate,
            InWatchList = model.InWatchList,
            Lot = model.Lot,
            FirstTradeDate = model.FirstTradeDate,
            LastTradeDate = model.LastTradeDate,
            FutureType = model.FutureType,
            AssetType = model.AssetType,
            BasicAsset = model.BasicAsset,
            BasicAssetSize = model.BasicAssetSize,
            InitialMarginOnBuy = model.InitialMarginOnBuy,
            InitialMarginOnSell = model.InitialMarginOnSell,
            MinPriceIncrementAmount = model.MinPriceIncrementAmount
        };

        return entity;
    }
    
    private Future GetModel(FutureEntity entity)
    {
        var model = new Future
        {
            Id = entity.Id,
            Ticker = entity.Ticker,
            LastPrice = entity.LastPrice,
            HighTargetPrice = entity.HighTargetPrice,
            LowTargetPrice = entity.LowTargetPrice,
            Figi = entity.Figi,
            InstrumentId = entity.InstrumentId,
            Name = entity.Name,
            ExpirationDate = entity.ExpirationDate,
            InWatchList = entity.InWatchList,
            Lot = entity.Lot,
            FirstTradeDate = entity.FirstTradeDate,
            LastTradeDate = entity.LastTradeDate,
            FutureType = entity.FutureType,
            AssetType = entity.AssetType,
            BasicAsset = entity.BasicAsset,
            BasicAssetSize = entity.BasicAssetSize,
            InitialMarginOnBuy = entity.InitialMarginOnBuy,
            InitialMarginOnSell = entity.InitialMarginOnSell,
            MinPriceIncrementAmount = entity.MinPriceIncrementAmount
        };

        return model;
    }
}