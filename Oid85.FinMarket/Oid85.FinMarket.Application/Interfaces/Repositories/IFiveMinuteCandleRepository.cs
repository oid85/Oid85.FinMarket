using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IFiveMinuteCandleRepository
{
    Task AddOrUpdateAsync(List<FiveMinuteCandle> candles);
    Task<FiveMinuteCandle?> GetLastAsync(Guid instrumentId);
    Task<List<FiveMinuteCandle>> GetLastWeekCandlesAsync(Guid instrumentId);
    Task<List<FiveMinuteCandle>> GetAsync(Guid instrumentId, DateOnly from, DateOnly to);
}