using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface ITickerRepository
{
    Task AddOrUpdateAsync(List<Ticker> tickers);
    Task<List<Ticker>> GetAllAsync();
    Task<Ticker?> GetByInstrumentIdAsync(Guid instrumentId);
    Task<Ticker?> GetByNameAsync(string name);
}