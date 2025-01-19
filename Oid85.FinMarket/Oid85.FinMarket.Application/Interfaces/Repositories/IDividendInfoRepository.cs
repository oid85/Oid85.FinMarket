using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IDividendInfoRepository
{
    Task AddOrUpdateAsync(List<DividendInfo> dividendInfos);
    Task<List<DividendInfo>> GetAllAsync();
    Task<List<DividendInfo>> GetAsync(List<Guid> instrumentIds, DateOnly from, DateOnly to);
}