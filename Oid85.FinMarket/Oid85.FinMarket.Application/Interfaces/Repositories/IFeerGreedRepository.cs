using Oid85.FinMarket.Domain.Models.Analyse;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IFeerGreedRepository
{
    Task AddAsync(List<FearGreedIndex> indexes);
    Task UpdateFieldsAsync(FearGreedIndex index);
    Task<List<FearGreedIndex>> GetAsync(DateOnly from, DateOnly to);
}