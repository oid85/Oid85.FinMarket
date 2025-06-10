namespace Oid85.FinMarket.Domain.Models.Algo;

public record StopLimit
{
    public int Quantity { get; set; }
    public double StopPrice { get; set; }
}