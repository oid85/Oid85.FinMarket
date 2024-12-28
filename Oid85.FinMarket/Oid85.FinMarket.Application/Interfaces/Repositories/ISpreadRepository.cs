using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface ISpreadRepository
{
    Task AddOrUpdateAsync(List<Spread> spreads);
    Task<List<Spread>> GetAllAsync();
    Task<List<Spread>> GetWatchListAsync();
    Task<Spread?> GetByTickerAsync(string firstInstrumentTicker);
}