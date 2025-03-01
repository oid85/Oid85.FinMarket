using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class BondCouponRepository(
    FinMarketContext context) 
    : IBondCouponRepository
{
    public async Task AddAsync(List<BondCoupon> bondCoupons)
    {
        if (bondCoupons is [])
            return;

        var entities = new List<BondCouponEntity>();
        
        foreach (var bondCoupon in bondCoupons)
            if (!await context.BondCouponEntities
                    .AnyAsync(x => 
                        x.InstrumentId == bondCoupon.InstrumentId
                        && x.CouponNumber == bondCoupon.CouponNumber))
                entities.Add(DataAccessMapper.Map(bondCoupon));

        await context.BondCouponEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
    
    public async Task<List<BondCoupon>> GetAllAsync() =>
        (await context.BondCouponEntities
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList(); 
    
    public async Task<List<BondCoupon>> GetAsync(
        DateOnly from, DateOnly to) =>
        (await context.BondCouponEntities
            .Where(x => 
                x.CouponDate >= from && 
                x.CouponDate <= to)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();
    
    public async Task<List<BondCoupon>> GetByInstrumentIdsAsync(List<Guid> instrumentIds) =>
        (await context.BondCouponEntities
            .Where(x => 
                instrumentIds.Contains(x.InstrumentId))
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();
}