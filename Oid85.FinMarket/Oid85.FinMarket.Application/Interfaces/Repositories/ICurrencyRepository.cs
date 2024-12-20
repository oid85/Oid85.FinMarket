using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface ICurrencyRepository
{
    Task AddOrUpdateAsync(List<Currency> currencies);
    Task<List<Currency>> GetAllAsync();
    Task<List<Currency>> GetWatchListAsync();
    Task<Currency?> GetByTickerAsync(string ticker);
}