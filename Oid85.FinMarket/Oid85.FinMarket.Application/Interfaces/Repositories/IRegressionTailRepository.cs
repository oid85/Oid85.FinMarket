using Oid85.FinMarket.Domain.Models.StatisticalArbitration;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IRegressionTailRepository
{
    Task AddAsync(RegressionTail regressionTail);
    Task UpdateAsync(string ticker1, string ticker2, List<double> tails, bool isStationary);
    Task<List<RegressionTail>> GetAllAsync();
    Task DeleteAsync();
}