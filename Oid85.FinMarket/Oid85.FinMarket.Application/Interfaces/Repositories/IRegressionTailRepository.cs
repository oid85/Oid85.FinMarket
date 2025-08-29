using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IRegressionTailRepository
{
    Task AddAsync(RegressionTail regressionTail);
    Task UpdateAsync(string tickerFirst, string tickerSecond, List<RegressionTailItem> tails, bool isStationary);
    Task<List<RegressionTail>> GetAllAsync();
    Task<RegressionTail?> GetAsync(string tickerFirst, string tickerSecond);
    Task DeleteAsync();
}