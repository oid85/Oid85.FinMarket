using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IHourlyCandleRepository
{
    Task AddOrUpdateAsync(List<HourlyCandle> candles);
}