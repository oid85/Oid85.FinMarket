namespace Oid85.FinMarket.Application.Models.Diagrams;

public class DiagramData<TAxisX, TAxisY>
{
    public string Title { get; set; } = string.Empty;
    public Axis<TAxisX> AxisX { get; set; } = new();
    public List<Axis<TAxisY>> AxisY { get; set; } = [];
}