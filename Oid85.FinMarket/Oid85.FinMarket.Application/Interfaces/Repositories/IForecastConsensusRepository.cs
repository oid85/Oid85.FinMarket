using System.Collections;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IForecastConsensusRepository
{
    Task AddAsync(List<ForecastConsensus> forecastConsensuses);
    Task<List<ForecastConsensus>> GetAllAsync();
    Task<List<ForecastConsensus>> GetAsync(List<Guid> instrumentIds);
}