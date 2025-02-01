using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IMultiplicatorRepository
{
    Task AddOrUpdateAsync(List<Multiplicator> multiplicators);
    Task UpdateSpreadAsync(Multiplicator multiplicator);
    Task<List<Multiplicator>> GetAllAsync();
    Task<Multiplicator?> GetAsync(string ticker);
}