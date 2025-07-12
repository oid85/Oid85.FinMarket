namespace Oid85.FinMarket.Domain.Models;

public class ShareMultiplicator
{
    public Guid Id { get; set; }
    public string Ticker { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double MarketCap { get; set; }
    public double Ev { get; set; }
    public double Revenue { get; set; }
}