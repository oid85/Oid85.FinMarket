using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IIndexRepository
{
    Task AddOrUpdateAsync(List<Indicative> indicatives);
    Task<List<Indicative>> GetAllAsync();
    Task<List<Indicative>> GetWatchListAsync();
    Task<Indicative?> GetByTickerAsync(string ticker);
}