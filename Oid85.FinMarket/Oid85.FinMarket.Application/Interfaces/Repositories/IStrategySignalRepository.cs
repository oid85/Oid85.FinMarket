using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IStrategySignalRepository
{
    Task AddAsync(StrategySignal strategySignal);
    Task UpdatePositionAsync(List<string> tickers, int countSignals);
    Task<List<StrategySignal>> GetAllAsync();
}