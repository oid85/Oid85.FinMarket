using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface ICandleRepository
{
    Task AddOrUpdateAsync(List<Candle> candles);
    Task<List<Candle>> GetCandlesAsync(string ticker, string timeframe);
}