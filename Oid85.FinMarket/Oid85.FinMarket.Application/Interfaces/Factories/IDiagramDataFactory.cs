using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Factories;

public interface IDiagramDataFactory
{
    Task<SimpleDiagramData> CreateDailyClosePricesDiagramDataAsync(List<Guid> instrumentIds, DateOnly from, DateOnly to);
    Task<BubbleDiagramData> CreateShareMultiplicatorsMCapPeNetDebtEbitdaAsync(List<Guid> instrumentIds);
    Task<BubbleDiagramData> CreateBankMultiplicatorsMCapPePbAsync(List<Guid> instrumentIds);
    Task<BacktestResultDiagramData> CreateBacktestResultDiagramDataAsync(Strategy strategy);
    Task<PairArbitrageBacktestResultDiagramData> CreatePairArbitrageBacktestResultDiagramDataAsync(PairArbitrageStrategy strategy);
    Task<BacktestResultDiagramData> CreateBacktestResultDiagramDataAsync(List<Strategy> strategies);
    Task<PairArbitrageBacktestResultDiagramData> CreatePairArbitrageBacktestResultDiagramDataAsync(List<PairArbitrageStrategy> strategies);
    Task<BacktestResultDiagramData> CreateBacktestResultWithoutPriceDiagramDataAsync(List<Strategy> strategies);
    Task<PairArbitrageBacktestResultDiagramData> CreatePairArbitrageBacktestResultWithoutPriceDiagramDataAsync(List<PairArbitrageStrategy> strategies);
    Task<SimpleDiagramData> CreateSpreadsDiagramDataAsync(DateOnly from, DateOnly to);
}