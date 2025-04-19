using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IFutureRepository
{
    Task AddAsync(List<Future> futures);
    Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice);
    Task<List<Future>> GetAllAsync();
    Task<List<Future>> GetAsync(List<string> tickers);
    Task<Future?> GetAsync(string ticker);
}