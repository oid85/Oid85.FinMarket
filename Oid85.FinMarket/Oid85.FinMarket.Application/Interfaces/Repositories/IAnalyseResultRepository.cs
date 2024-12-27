using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IAnalyseResultRepository
{
    Task AddOrUpdateAsync(List<AnalyseResult> results);
    Task<List<AnalyseResult>> GetAsync(Guid instrumentId, DateTime from, DateTime to);
    Task<List<AnalyseResult>> GetAsync(List<Guid> instrumentIds, DateTime from, DateTime to);
    Task<AnalyseResult?> GetLastAsync(Guid instrumentId);
}