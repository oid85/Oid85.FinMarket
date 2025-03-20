namespace Oid85.FinMarket.Application.Models.Diagrams;

public class DataPointSeries
{
    public string Title { get; set; } = string.Empty;
    public List<DataPoint> Series { get; set; } = [];
}