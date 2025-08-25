using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IRegressionTailRepository
{
    Task AddAsync(RegressionTail regressionTail);
    Task UpdateAsync(string ticker1, string ticker2, List<RegressionTailItem> tails, bool isStationary);
    Task<List<RegressionTail>> GetAllAsync();
    Task<RegressionTail?> GetAsync(string ticker1, string ticker2);
    Task DeleteAsync();
}