using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Factories;

public interface IDiagramDataFactory
{
    Task<SimpleDiagramData> CreateDailyClosePricesDiagramDataAsync(List<Guid> instrumentIds, DateOnly from, DateOnly to);
    Task<BubbleDiagramData> CreateMultiplicatorsMCapPeNetDebtEbitdaAsync(List<Guid> instrumentIds);
    Task<BacktestResultDiagramData> CreateBacktestResultDiagramDataAsync(Strategy strategy);
    Task<BacktestResultDiagramData> CreateBacktestResultDiagramDataAsync(List<Strategy> strategies);
}