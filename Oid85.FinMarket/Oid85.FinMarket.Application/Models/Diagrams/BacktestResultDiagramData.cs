namespace Oid85.FinMarket.Application.Models.Diagrams;

public class BacktestResultDiagramData
{
    public string Title { get; set; } = string.Empty;
    public BacktestResultDataPointSeries Data { get; set; } = new();
}