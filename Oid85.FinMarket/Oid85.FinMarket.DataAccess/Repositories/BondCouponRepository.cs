using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class BondCouponRepository(
    FinMarketContext context,
    IMapper mapper) : IBondCouponRepository
{
    public async Task AddOrUpdateAsync(List<BondCoupon> bondCoupons)
    {
        if (!bondCoupons.Any())
            return;
        
        foreach (var bondCoupon in bondCoupons)
        {
            var entity = context.BondCouponEntities
                .FirstOrDefault(x => 
                    x.Ticker == bondCoupon.Ticker &&
                    x.CouponNumber == bondCoupon.CouponNumber);

            if (entity is null)
            {
                entity = mapper.Map<BondCouponEntity>(bondCoupon);
                await context.BondCouponEntities.AddAsync(entity);
            }

            else
            {
                entity.Ticker = bondCoupon.Ticker;
                entity.CouponDate = bondCoupon.CouponDate;
                entity.CouponNumber = bondCoupon.CouponNumber;
                entity.CouponPeriod = bondCoupon.CouponPeriod;
                entity.CouponStartDate = bondCoupon.CouponStartDate;
                entity.CouponEndDate = bondCoupon.CouponEndDate;
                entity.PayOneBond = bondCoupon.PayOneBond;
            }
        }

        await context.SaveChangesAsync();
    }
    
    public Task<List<BondCoupon>> GetBondCouponsAsync() =>
        context.BondEntities
            .Select(x => mapper.Map<BondCoupon>(x))
            .ToListAsync();    
}