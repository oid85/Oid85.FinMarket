using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IForecastTargetRepository
{
    Task AddAsync(List<ForecastTarget> forecastTargets);
    Task UpdateForecastTargetAsync(Guid instrumentId, ForecastTarget forecastTarget);
    Task<List<ForecastTarget>> GetAllAsync();
    Task<List<ForecastTarget>> GetByTickerAsync(string ticker);
    Task<List<ForecastTarget>> GetByInstrumentIdAsync(Guid instrumentId);
}