using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IInstrumentRepository
{
    Task AddOrUpdateAsync(List<Instrument> tickers);
    Task<Instrument?> GetByInstrumentIdAsync(Guid instrumentId);
    Task<Instrument?> GetByTickerAsync(string ticker);
}