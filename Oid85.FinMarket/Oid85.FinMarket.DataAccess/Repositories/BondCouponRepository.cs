using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class BondCouponRepository(
    ILogger logger,
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
            else
                await UpdateFieldsAsync(bondCoupon);

        await context.BondCouponEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
 
    private async Task UpdateFieldsAsync(BondCoupon bondCoupon)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.BondCouponEntities
                .Where(x => 
                    x.InstrumentId == bondCoupon.InstrumentId &&
                    x.CouponNumber == bondCoupon.CouponNumber)
                .ExecuteUpdateAsync(x => x
                        .SetProperty(entity => entity.PayOneBond, bondCoupon.PayOneBond));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }
    
    public async Task<List<BondCoupon>> GetAsync(List<Guid> instrumentIds) =>
        (await context.BondCouponEntities
            .Where(x => 
                instrumentIds.Contains(x.InstrumentId))
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();
}