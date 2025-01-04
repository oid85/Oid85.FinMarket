using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IAnalyseResultRepository
{
    Task AddAsync(List<AnalyseResult> results);
    Task<List<AnalyseResult>> GetAsync(Guid instrumentId, DateOnly from, DateOnly to);
    Task<List<AnalyseResult>> GetAsync(List<Guid> instrumentIds, DateOnly from, DateOnly to);
    Task<AnalyseResult?> GetLastAsync(Guid instrumentId, string analyseType);
}