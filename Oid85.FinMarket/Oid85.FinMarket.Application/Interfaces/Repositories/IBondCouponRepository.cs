using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IBondCouponRepository
{
    Task AddAsync(List<BondCoupon> bondCoupons);
    Task<List<BondCoupon>> GetAllAsync();
    Task<List<BondCoupon>> GetAsync(DateOnly from, DateOnly to);
    Task<List<BondCoupon>> GetByInstrumentIdsAsync(List<Guid> instrumentIds);
}