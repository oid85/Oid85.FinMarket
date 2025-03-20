namespace Oid85.FinMarket.Application.Models.Diagrams;

public class SimpleDiagramData
{
    public string Title { get; set; } = string.Empty;
    public List<DataPointSeries> Data { get; set; } = new();
}