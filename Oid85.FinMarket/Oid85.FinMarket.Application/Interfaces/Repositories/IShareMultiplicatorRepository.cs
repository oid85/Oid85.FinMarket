using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IShareMultiplicatorRepository
{
    Task AddOrUpdateAsync(List<ShareMultiplicator> multiplicators);
    Task UpdateFieldsAsync(ShareMultiplicator shareMultiplicator);
    Task<List<ShareMultiplicator>> GetAllAsync();
    Task<ShareMultiplicator?> GetAsync(string ticker);
    Task<List<ShareMultiplicator>> GetAsync(List<Guid> instrumentIds);
}