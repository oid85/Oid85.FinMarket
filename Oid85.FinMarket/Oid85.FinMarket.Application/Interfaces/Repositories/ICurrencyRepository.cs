using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface ICurrencyRepository
{
    Task AddAsync(List<Currency> currencies);
    Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice);
    Task<List<Currency>> GetAllAsync();
    Task<List<Currency>> GetWatchListAsync();
    Task<Currency?> GetByTickerAsync(string ticker);
    Task<Currency?> GetByInstrumentIdAsync(Guid instrumentId);
}