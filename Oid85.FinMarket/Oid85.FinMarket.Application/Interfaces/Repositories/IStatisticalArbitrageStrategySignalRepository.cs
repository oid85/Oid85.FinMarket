using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IStatisticalArbitrageStrategySignalRepository
{
    Task AddAsync(StatisticalArbitrageStrategySignal strategySignal);
    Task UpdatePositionAsync(StatisticalArbitrageStrategySignal strategySignal);
    Task<List<StatisticalArbitrageStrategySignal>> GetAllAsync();
}