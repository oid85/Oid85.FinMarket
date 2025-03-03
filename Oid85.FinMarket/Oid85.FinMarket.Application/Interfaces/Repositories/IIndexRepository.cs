using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IIndexRepository
{
    Task AddAsync(List<FinIndex> indexes);
    Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice);
    Task<List<FinIndex>> GetByTickersAsync(List<string> tickers);
}