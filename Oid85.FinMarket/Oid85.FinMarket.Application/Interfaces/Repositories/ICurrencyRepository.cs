using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface ICurrencyRepository
{
    Task AddAsync(List<Currency> currencies);
    Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice);
    Task<List<Currency>> GetAsync(List<string> tickers);
}