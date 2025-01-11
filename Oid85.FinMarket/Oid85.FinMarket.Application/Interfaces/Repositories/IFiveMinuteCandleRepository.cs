using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IFiveMinuteCandleRepository
{
    Task AddOrUpdateAsync(List<FiveMinuteCandle> candles);
    Task<List<FiveMinuteCandle>> GetAsync(Guid instrumentId);
    Task<List<FiveMinuteCandle>> GetAsync(Guid instrumentId, DateTime from, DateTime to);
    Task<FiveMinuteCandle?> GetLastAsync(Guid instrumentId);
}