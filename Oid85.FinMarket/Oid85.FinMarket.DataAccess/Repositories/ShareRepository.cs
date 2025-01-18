using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Logging.Services;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ShareRepository(
    ILogService logService,
    FinMarketContext context) 
    : IShareRepository
{
    public async Task AddAsync(List<Share> shares)
    {
        if (shares is [])
            return;

        var entities = new List<ShareEntity>();
        
        foreach (var share in shares)
            if (!await context.ShareEntities
                    .AnyAsync(x => x.InstrumentId == share.InstrumentId))
                entities.Add(GetEntity(share));

        await context.ShareEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.ShareEntities
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

    public async Task<List<Share>> GetAllAsync() =>
        (await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<Share>> GetWatchListAsync() =>
        (await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Share?> GetByTickerAsync(string ticker)
    {
        var entity = await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    public async Task<Share?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);

        return entity is null 
            ? null 
            : GetModel(entity);
    }

    private ShareEntity GetEntity(Share model)
    {
        var entity = new ShareEntity
        {
            Ticker = model.Ticker,
            LastPrice = model.LastPrice,
            HighTargetPrice = model.HighTargetPrice,
            LowTargetPrice = model.LowTargetPrice,
            Isin = model.Isin,
            Figi = model.Figi,
            InstrumentId = model.InstrumentId,
            Name = model.Name,
            Sector = model.Sector,
            InWatchList = model.InWatchList
        };

        return entity;
    }
    
    private Share GetModel(ShareEntity entity)
    {
        var model = new Share
        {
            Id = entity.Id,
            Ticker = entity.Ticker,
            LastPrice = entity.LastPrice,
            HighTargetPrice = entity.HighTargetPrice,
            LowTargetPrice = entity.LowTargetPrice,
            Isin = entity.Isin,
            Figi = entity.Figi,
            InstrumentId = entity.InstrumentId,
            Name = entity.Name,
            Sector = entity.Sector,
            InWatchList = entity.InWatchList
        };

        return model;
    }
}