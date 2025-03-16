using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IFeerGreedRepository
{
    Task AddAsync(List<FearGreedIndex> indexes);
    Task<List<FearGreedIndex>> GetLastYearAsync();
}