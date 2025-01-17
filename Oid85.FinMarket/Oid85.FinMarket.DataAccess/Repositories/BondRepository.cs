using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Logging.Services;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class BondRepository(
    ILogService logService,
    FinMarketContext context) 
    : IBondRepository
{
    public async Task AddAsync(List<Bond> bonds)
    {        
        if (bonds is [])
            return;

        var entities = new List<BondEntity>();
        
        foreach (var bond in bonds)
            if (!await context.BondEntities
                    .AnyAsync(x => x.InstrumentId == bond.InstrumentId))
                entities.Add(GetEntity(bond));

        await context.BondEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.BondEntities
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

    public async Task<List<Bond>> GetAllAsync() =>
        (await context.BondEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<Bond>> GetWatchListAsync() =>
        (await context.BondEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Bond?> GetByTickerAsync(string ticker)
    {
        var entity = await context.BondEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private BondEntity GetEntity(Bond model)
    {
        var entity = new BondEntity();
        
        entity.Ticker = model.Ticker;
        entity.LastPrice = model.LastPrice;
        entity.Isin = model.Isin;
        entity.Figi = model.Figi;
        entity.InstrumentId = model.InstrumentId;
        entity.Name = model.Name;
        entity.Sector = model.Sector;
        entity.InWatchList = model.InWatchList;
        entity.Nkd = model.Nkd;
        entity.MaturityDate = model.MaturityDate;
        entity.FloatingCouponFlag = model.FloatingCouponFlag;

        return entity;
    }
    
    private Bond GetModel(BondEntity entity)
    {
        var model = new Bond();
        
        model.Id = entity.Id;
        model.Ticker = entity.Ticker;
        model.LastPrice = entity.LastPrice;
        model.Isin = entity.Isin;
        model.Figi = entity.Figi;
        model.InstrumentId = entity.InstrumentId;
        model.Name = entity.Name;
        model.Sector = entity.Sector;
        model.InWatchList = entity.InWatchList;
        model.Nkd = entity.Nkd;
        model.MaturityDate = entity.MaturityDate;
        model.FloatingCouponFlag = entity.FloatingCouponFlag;

        return model;
    }
}