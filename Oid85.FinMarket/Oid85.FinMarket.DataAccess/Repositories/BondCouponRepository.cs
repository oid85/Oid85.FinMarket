using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class BondCouponRepository(
    FinMarketContext context) 
    : IBondCouponRepository
{
    public async Task AddAsync(List<BondCoupon> bondCoupons)
    {
        if (bondCoupons.Count == 0)
            return;

        var entities = new List<BondCouponEntity>();
        
        foreach (var bondCoupon in bondCoupons)
            if (!await context.BondCouponEntities
                    .AnyAsync(x => 
                        x.InstrumentId == bondCoupon.InstrumentId
                        && x.CouponNumber == bondCoupon.CouponNumber))
                entities.Add(GetEntity(bondCoupon));

        await context.BondCouponEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
    
    public async Task<List<BondCoupon>> GetAllAsync() =>
        (await context.BondCouponEntities
            .ToListAsync())
        .Select(GetModel)
        .ToList(); 
    
    public async Task<List<BondCoupon>> GetAsync(
        DateOnly from, DateOnly to) =>
        (await context.BondCouponEntities
            .Where(x => 
                x.CouponDate >= from && 
                x.CouponDate <= to)
            .ToListAsync())
        .Select(GetModel)
        .ToList();
    
    private BondCouponEntity GetEntity(BondCoupon model)
    {
        var entity = new BondCouponEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Ticker = model.Ticker;
        entity.CouponDate = model.CouponDate;
        entity.CouponNumber = model.CouponNumber;
        entity.CouponPeriod = model.CouponPeriod;
        entity.CouponStartDate = model.CouponStartDate;
        entity.CouponEndDate = model.CouponEndDate;
        entity.PayOneBond = model.PayOneBond;

        return entity;
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