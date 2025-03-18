namespace Oid85.FinMarket.Application.Models.Diagrams;

public class Axis<T>
{
    public string Title { get; set; } = string.Empty;
    public List<T> Values { get; set; } = [];
}