using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IInstrumentRepository
{
    Task AddOrUpdateAsync(List<Instrument> tickers);
    Task<List<Instrument>> GetAllAsync();
    Task<Instrument?> GetByInstrumentIdAsync(Guid instrumentId);
    Task<Instrument?> GetByNameAsync(string name);
    Task<Instrument?> GetByTickerAsync(string ticker);
    Task<(double LowTargetPrice, double HighTargetPrice)> GetTargetPricesAsync(Guid instrumentId);
}