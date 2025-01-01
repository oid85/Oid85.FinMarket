using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class BondRepository(
    FinMarketContext context) : IBondRepository
{
    public async Task AddOrUpdateAsync(List<Bond> bonds)
    {        
        if (bonds.Count == 0)
            return;
        
        foreach (var bond in bonds)
        {
            var entity = context.BondEntities
                .FirstOrDefault(x => 
                    x.InstrumentId == bond.InstrumentId);

            if (entity is null)
            {
                SetEntity(ref entity, bond);
                
                if (entity is not null)
                    await context.BondEntities.AddAsync(entity);
            }

            else
            {
                SetEntity(ref entity, bond);
            }
        }
        
        await context.SaveChangesAsync();
    }

    public async Task<List<Bond>> GetAllAsync() =>
        (await context.BondEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<Bond>> GetWatchListAsync() =>
        (await context.BondEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Bond?> GetByTickerAsync(string ticker)
    {
        var entity = await context.BondEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private void SetEntity(ref BondEntity? entity, Bond model)
    {
        entity ??= new BondEntity();
        
        entity.Ticker = model.Ticker;
        entity.Price = model.Price;
        entity.Isin = model.Isin;
        entity.Figi = model.Figi;
        entity.InstrumentId = model.InstrumentId;
        entity.Name = model.Name;
        entity.Sector = model.Sector;
        entity.InWatchList = model.InWatchList;
        entity.Nkd = model.Nkd;
        entity.MaturityDate = model.MaturityDate;
        entity.FloatingCouponFlag = model.FloatingCouponFlag;
    }
    
    private Bond GetModel(BondEntity entity)
    {
        var model = new Bond();
        
        model.Id = entity.Id;
        model.Ticker = entity.Ticker;
        model.Price = entity.Price;
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