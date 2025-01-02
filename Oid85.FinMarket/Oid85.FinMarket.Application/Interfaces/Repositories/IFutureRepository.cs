using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IFutureRepository
{
    Task AddAsync(List<Future> futures);
    Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice);
    Task<List<Future>> GetAllAsync();
    Task<List<Future>> GetWatchListAsync();
    Task<Future?> GetByTickerAsync(string ticker);
    Task<Future?> GetByInstrumentIdAsync(Guid instrumentId);
}