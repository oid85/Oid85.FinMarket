using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IInstrumentRepository
{
    Task AddOrUpdateAsync(List<Instrument> tickers);
    Task<Instrument?> GetAsync(Guid instrumentId);
    Task<List<Instrument>> GetAsync(List<Guid> instrumentIds);
    Task<Instrument?> GetAsync(string ticker);
}