using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IForecastConsensusRepository
{
    Task AddAsync(List<ForecastConsensus> forecastConsensuses);
    Task UpdateForecastTargetAsync(Guid instrumentId, ForecastConsensus forecastConsensus);
    Task<List<ForecastConsensus>> GetAllAsync();
    Task<ForecastConsensus?> GetByTickerAsync(string ticker);
    Task<ForecastConsensus?> GetByInstrumentIdAsync(Guid instrumentId);
}