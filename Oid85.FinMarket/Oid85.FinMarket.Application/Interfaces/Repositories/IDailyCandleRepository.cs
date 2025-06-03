using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IDailyCandleRepository
{
    Task AddOrUpdateAsync(List<DailyCandle> candles);
    Task<DailyCandle?> GetLastAsync(Guid instrumentId);
    Task<List<DailyCandle>> GetLastYearAsync(Guid instrumentId);
    Task<List<DailyCandle>> GetLastTwoYearsAsync(Guid instrumentId);
    Task<DailyCandle?> GetAsync(Guid instrumentId, DateOnly date);
    Task<List<DailyCandle>> GetAsync(Guid instrumentId, DateOnly from, DateOnly to);
    Task<List<DailyCandle>> GetAsync(string ticker, DateOnly from, DateOnly to);
}