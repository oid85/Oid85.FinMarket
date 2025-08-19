using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface ICorrelationRepository
{
    Task AddAsync(Correlation correlation);
    Task UpdateAsync(string ticker1, string ticker2, double value);
    Task<List<Correlation>> GetAllAsync();
    Task DeleteAsync();
}