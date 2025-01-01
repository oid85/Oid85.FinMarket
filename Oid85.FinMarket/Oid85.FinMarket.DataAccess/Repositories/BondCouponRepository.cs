using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class BondCouponRepository(
    FinMarketContext context) : IBondCouponRepository
{
    public async Task AddOrUpdateAsync(List<BondCoupon> bondCoupons)
    {
        if (bondCoupons.Count == 0)
            return;
        
        foreach (var bondCoupon in bondCoupons)
        {
            var entity = context.BondCouponEntities
                .FirstOrDefault(x => 
                    x.InstrumentId == bondCoupon.InstrumentId &&
                    x.CouponNumber == bondCoupon.CouponNumber);

            if (entity is null)
            {
                SetEntity(ref entity, bondCoupon);
                
                if (entity is not null)
                    await context.BondCouponEntities.AddAsync(entity);
            }

            else
            {
                SetEntity(ref entity, bondCoupon);
            }
        }

        await context.SaveChangesAsync();
    }
    
    public async Task<List<BondCoupon>> GetAllAsync() =>
        (await context.BondCouponEntities
            .ToListAsync())
        .Select(GetModel)
        .ToList(); 
    
    public async Task<List<BondCoupon>> GetAsync(
        DateTime from, DateTime to) =>
        (await context.BondCouponEntities
            .Where(x => 
                x.CouponDate >= DateOnly.FromDateTime(from) && 
                x.CouponDate <= DateOnly.FromDateTime(to))
            .ToListAsync())
        .Select(GetModel)
        .ToList();
    
    private void SetEntity(ref BondCouponEntity? entity, BondCoupon model)
    {
        entity ??= new BondCouponEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Ticker = model.Ticker;
        entity.CouponDate = model.CouponDate;
        entity.CouponNumber = model.CouponNumber;
        entity.CouponPeriod = model.CouponPeriod;
        entity.CouponStartDate = model.CouponStartDate;
        entity.CouponEndDate = model.CouponEndDate;
        entity.PayOneBond = model.PayOneBond;
    }
    
    private BondCoupon GetModel(BondCouponEntity entity)
    {
        var model = new BondCoupon();
        
        model.Id = entity.Id;
        model.InstrumentId = entity.InstrumentId;
        model.Ticker = entity.Ticker;
        model.CouponDate = entity.CouponDate;
        model.CouponNumber = entity.CouponNumber;
        model.CouponPeriod = entity.CouponPeriod;
        model.CouponStartDate = entity.CouponStartDate;
        model.CouponEndDate = entity.CouponEndDate;
        model.PayOneBond = entity.PayOneBond;

        return model;
    }
}