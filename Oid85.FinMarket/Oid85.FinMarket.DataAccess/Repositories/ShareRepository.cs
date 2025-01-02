using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ShareRepository(
    FinMarketContext context) 
    : IShareRepository
{
    public async Task AddAsync(List<Share> shares)
    {
        if (shares.Count == 0)
            return;

        var entities = new List<ShareEntity>();
        
        foreach (var share in shares)
            if (!await context.ShareEntities
                    .AnyAsync(x => x.InstrumentId == share.InstrumentId))
                entities.Add(GetEntity(share));

        await context.ShareEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice) =>
        context.ShareEntities
            .Where(x => x.InstrumentId == instrumentId)
            .ExecuteUpdateAsync(
                s => s.SetProperty(
                    u => u.Price, lastPrice));

    public async Task<List<Share>> GetAllAsync() =>
        (await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<Share>> GetWatchListAsync() =>
        (await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Share?> GetByTickerAsync(string ticker)
    {
        var entity = await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    public async Task<Share?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);

        return entity is null 
            ? null 
            : GetModel(entity);
    }

    private ShareEntity GetEntity(Share model)
    {
        var entity = new ShareEntity();
        
        entity.Ticker = model.Ticker;
        entity.Price = model.Price;
        entity.Isin = model.Isin;
        entity.Figi = model.Figi;
        entity.InstrumentId = model.InstrumentId;
        entity.Name = model.Name;
        entity.Sector = model.Sector;
        entity.InWatchList = model.InWatchList;

        return entity;
    }
    
    private Share GetModel(ShareEntity entity)
    {
        var model = new Share();
        
        model.Id = entity.Id;
        model.Ticker = entity.Ticker;
        model.Price = entity.Price;
        model.Isin = entity.Isin;
        model.Figi = entity.Figi;
        model.InstrumentId = entity.InstrumentId;
        model.Name = entity.Name;
        model.Sector = entity.Sector;
        model.InWatchList = entity.InWatchList;

        return model;
    }
}