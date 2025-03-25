namespace Oid85.FinMarket.Application.Models.Diagrams;

public class BubbleDiagramData
{
    public string Title { get; set; } = string.Empty;
    public List<BubbleDataPoint> Series { get; set; } = [];
}