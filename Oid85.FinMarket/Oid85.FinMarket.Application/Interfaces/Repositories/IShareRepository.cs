using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IShareRepository
{
    Task AddOrUpdateAsync(List<Share> shares);
    Task<List<Share>> GetSharesAsync();
    Task<List<Share>> GetMoexIndexSharesAsync();
    Task<List<Share>> GetPortfolioSharesAsync();
    Task<List<Share>> GetWatchListSharesAsync();
    Task<Share?> GetShareByTickerAsync(string ticker);
}