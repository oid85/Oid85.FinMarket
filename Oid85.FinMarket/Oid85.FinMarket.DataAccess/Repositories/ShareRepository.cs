using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ShareRepository(
    FinMarketContext context) : IShareRepository
{
    public async Task AddOrUpdateAsync(List<Share> shares)
    {
        if (shares.Count == 0)
            return;
        
        foreach (var share in shares)
        {
            var entity = context.ShareEntities
                .FirstOrDefault(x => 
                    x.InstrumentId == share.InstrumentId);

            if (entity is null)
            {
                SetEntity(ref entity, share);
                
                if (entity is not null)
                    await context.ShareEntities.AddAsync(entity);
            }

            else
            {
                SetEntity(ref entity, share);
            }
        }

        await context.SaveChangesAsync();
    }

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

    private void SetEntity(ref ShareEntity? entity, Share model)
    {
        entity ??= new ShareEntity();
        
        entity.Ticker = model.Ticker;
        entity.Price = model.Price;
        entity.Isin = model.Isin;
        entity.Figi = model.Figi;
        entity.InstrumentId = model.InstrumentId;
        entity.Name = model.Name;
        entity.Sector = model.Sector;
        entity.InWatchList = model.InWatchList;
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