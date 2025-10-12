using Oid85.FinMarket.Domain.Models.PairArbitrage;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IPairArbitrageStrategySignalRepository
{
    Task AddAsync(PairArbitrageStrategySignal strategySignal);
    Task UpdatePositionAsync(PairArbitrageStrategySignal strategySignal);
    Task<List<PairArbitrageStrategySignal>> GetAllAsync();
}