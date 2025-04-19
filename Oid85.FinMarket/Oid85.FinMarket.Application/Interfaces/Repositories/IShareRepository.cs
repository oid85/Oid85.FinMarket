using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IShareRepository
{
    Task AddAsync(List<Share> shares);
    Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice);
    Task<List<Share>> GetAsync(List<string> tickers);
    Task<Share?> GetAsync(string ticker);
    Task<Share?> GetAsync(Guid instrumentId);
    Task<List<Share>> GetAsync(List<Guid> instrumentIds);
}