namespace Oid85.FinMarket.Domain.Models.Algo;

public class ArbitrageGraphPoint
{
    public double? PriceFirst { get; set; } = null;
    public double? PriceSecond { get; set; } = null;
    public double? Spread { get; set; } = null;
}