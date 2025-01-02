using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IBondRepository
{
    Task AddAsync(List<Bond> bonds);
    Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice);
    Task<List<Bond>> GetAllAsync();
    Task<List<Bond>> GetWatchListAsync();
    Task<Bond?> GetByTickerAsync(string ticker);
}