using System.Collections;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IAnalyseResultRepository
{
    Task AddOrUpdateAsync(List<AnalyseResult> results);
    Task<List<AnalyseResult>> GetAnalyseResultsAsync(string ticker, DateTime from, DateTime to);
}