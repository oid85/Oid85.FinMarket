using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IShareRepository
{
    Task AddOrUpdateAsync(List<Share> shares);
    Task<List<Share>> GetAllAsync();
    Task<List<Share>> GetWatchListAsync();
    Task<Share?> GetByTickerAsync(string ticker);
}