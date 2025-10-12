using Oid85.FinMarket.Domain.Models.Analyse;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IBankMultiplicatorRepository
{
    Task AddOrUpdateAsync(List<BankMultiplicator> multiplicators);
    Task UpdateFieldsAsync(BankMultiplicator multiplicator);
    Task<List<BankMultiplicator>> GetAllAsync();
    Task<BankMultiplicator?> GetAsync(string ticker);
    Task<List<BankMultiplicator>> GetAsync(List<Guid> instrumentIds);
}