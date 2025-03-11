using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface ICandleRepository
{
    Task AddOrUpdateAsync(List<Candle> candles);
    Task<Candle?> GetLastAsync(Guid instrumentId);
    Task<List<Candle>> GetLastYearAsync(Guid instrumentId);
    Task<Candle?> GetAsync(Guid instrumentId, DateOnly date);
}