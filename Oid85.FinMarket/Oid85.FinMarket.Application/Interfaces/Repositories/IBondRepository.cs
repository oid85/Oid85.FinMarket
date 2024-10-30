using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IBondRepository
{
    Task AddOrUpdateAsync(List<Bond> bonds);
    Task<List<Bond>> GetBondsAsync();
}