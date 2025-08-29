using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface ICorrelationRepository
{
    Task AddAsync(Correlation correlation);
    Task UpdateAsync(string tickerFirst, string tickerSecond, double value);
    Task<List<Correlation>> GetAllAsync();
    Task DeleteAsync();
}