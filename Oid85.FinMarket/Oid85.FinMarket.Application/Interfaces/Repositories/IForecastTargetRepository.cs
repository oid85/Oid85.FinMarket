using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IForecastTargetRepository
{
    Task AddAsync(List<ForecastTarget> forecastTargets);
    Task<List<ForecastTarget>> GetAllAsync();
    Task<List<ForecastTarget>> GetAsync(List<Guid> instrumentIds);
}