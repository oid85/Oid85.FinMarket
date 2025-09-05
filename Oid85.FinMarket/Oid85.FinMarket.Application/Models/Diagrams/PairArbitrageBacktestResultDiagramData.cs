namespace Oid85.FinMarket.Application.Models.Diagrams;

public class PairArbitrageBacktestResultDiagramData
{
    public string Title { get; set; } = string.Empty;
    public PairArbitrageBacktestResultDataPointSeries Data { get; set; } = new();
}