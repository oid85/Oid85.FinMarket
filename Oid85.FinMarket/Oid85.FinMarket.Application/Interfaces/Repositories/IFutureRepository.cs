using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IFutureRepository
{
    Task AddOrUpdateAsync(List<Future> futures);
    Task<List<Future>> GetAllAsync();
    Task<List<Future>> GetWatchListAsync();
    Task<Future?> GetByTickerAsync(string ticker);
}