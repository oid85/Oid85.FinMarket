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
        var entity = new BondEntity
        {
            Ticker = model.Ticker,
            LastPrice = model.LastPrice,
            Isin = model.Isin,
            Figi = model.Figi,
            InstrumentId = model.InstrumentId,
            Name = model.Name,
            Sector = model.Sector,
            InWatchList = model.InWatchList,
            Nkd = model.Nkd,
            MaturityDate = model.MaturityDate,
            FloatingCouponFlag = model.FloatingCouponFlag
        };

        return entity;
    }
    
    private Bond GetModel(BondEntity entity)
    {
        var model = new Bond
        {
            Id = entity.Id,
            Ticker = entity.Ticker,
            LastPrice = entity.LastPrice,
            Isin = entity.Isin,
            Figi = entity.Figi,
            InstrumentId = entity.InstrumentId,
            Name = entity.Name,
            Sector = entity.Sector,
            InWatchList = entity.InWatchList,
            Nkd = entity.Nkd,
            MaturityDate = entity.MaturityDate,
            FloatingCouponFlag = entity.FloatingCouponFlag
        };

        return model;
    }
}