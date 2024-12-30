using Oid85.FinMarket.Domain.Models;
using Index = Oid85.FinMarket.Domain.Models.Index;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IIndexRepository
{
    Task AddOrUpdateAsync(List<Index> indicatives);
    Task<List<Index>> GetAllAsync();
    Task<List<Index>> GetWatchListAsync();
    Task<Index?> GetByTickerAsync(string ticker);
}