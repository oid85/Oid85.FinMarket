using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IFutureRepository
{
    Task AddOrUpdateAsync(List<Future> futures);
    Task<List<Future>> GetFuturesAsync();
    Task<List<Future>> GetPortfolioFuturesAsync();
    Task<List<Future>> GetWatchListFuturesAsync();
    Task<Future?> GetFutureByTickerAsync(string ticker);
}