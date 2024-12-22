using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IAnalyseResultRepository
{
    Task AddOrUpdateAsync(List<AnalyseResult> results);
    Task<List<AnalyseResult>> GetAsync(string ticker, DateTime from, DateTime to);
    Task<List<AnalyseResult>> GetAsync(List<string> tickers, DateTime from, DateTime to);
    Task<AnalyseResult?> GetLastAsync(string ticker, string timeframe);
}