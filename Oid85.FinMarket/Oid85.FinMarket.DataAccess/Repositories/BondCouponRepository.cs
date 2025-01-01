using Mapster;
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
                entity = bondCoupon.Adapt<BondCouponEntity>();
                await context.BondCouponEntities.AddAsync(entity);
            }

            else
            {
                entity.Adapt(bondCoupon);
            }
        }

        await context.SaveChangesAsync();
    }
    
    public async Task<List<BondCoupon>> GetAllAsync() =>
        (await context.BondCouponEntities
            .ToListAsync())
        .Select(x => x.Adapt<BondCoupon>())
        .ToList(); 
    
    public async Task<List<BondCoupon>> GetAsync(
        DateTime from, DateTime to) =>
        (await context.BondCouponEntities
            .Where(x => 
                x.CouponDate >= DateOnly.FromDateTime(from) && 
                x.CouponDate <= DateOnly.FromDateTime(to))
            .ToListAsync())
        .Select(x => x.Adapt<BondCoupon>())
        .ToList();   
}