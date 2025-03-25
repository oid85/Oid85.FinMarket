namespace Oid85.FinMarket.Application.Models.Diagrams;

public class SimpleDataPointSeries
{
    public string Title { get; set; } = string.Empty;
    public List<SimpleDataPoint> Series { get; set; } = [];
}