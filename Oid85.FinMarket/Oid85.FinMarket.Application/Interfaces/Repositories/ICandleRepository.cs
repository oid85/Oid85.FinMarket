using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface ICandleRepository
{
    Task AddOrUpdateAsync(List<Candle> candles);
    Task<List<Candle>> GetAsync(string ticker, string timeframe);
    Task<Candle?> GetLastAsync(string ticker, string timeframe);
}