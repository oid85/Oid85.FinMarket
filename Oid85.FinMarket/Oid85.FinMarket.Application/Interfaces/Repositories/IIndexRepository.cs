using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IIndexRepository
{
    Task AddAsync(List<FinIndex> indicatives);
    Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice);
    Task<List<FinIndex>> GetAllAsync();
    Task<List<FinIndex>> GetWatchListAsync();
    Task<FinIndex?> GetByTickerAsync(string ticker);
}