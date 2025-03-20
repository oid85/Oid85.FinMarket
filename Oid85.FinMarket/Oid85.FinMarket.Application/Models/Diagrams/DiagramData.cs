namespace Oid85.FinMarket.Application.Models.Diagrams;

public class DiagramData<TXValues, TYValues>
{
    public string Title { get; set; } = string.Empty;
    public Axis<TXValues> AxisX { get; set; } = new();
    public List<Axis<TYValues?>> AxisesY { get; set; } = [];
}