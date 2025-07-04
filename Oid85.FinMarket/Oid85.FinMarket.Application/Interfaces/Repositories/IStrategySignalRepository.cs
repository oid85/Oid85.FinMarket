using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IStrategySignalRepository
{
    Task AddAsync(StrategySignal strategySignal);
    Task UpdatePositionAsync(string ticker, int countSignals, double positionCost, int positionSize, double lastPrice);
    Task<List<StrategySignal>> GetAllAsync();
}