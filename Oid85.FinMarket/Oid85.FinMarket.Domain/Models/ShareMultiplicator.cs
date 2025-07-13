namespace Oid85.FinMarket.Domain.Models;

public class ShareMultiplicator
{
    public Guid Id { get; set; }
    public string Ticker { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double MarketCap { get; set; }
    public double Ev { get; set; }
    public double Revenue { get; set; }
    public double NetIncome { get; set; }
    public double DdAo { get; set; }
    public double DdAp { get; set; }
    public double DdNetIncome { get; set; }
    public double Pe { get; set; }
    public double Ps { get; set; }
    public double Pb { get; set; }
    public double EvEbitda { get; set; }
    public double EbitdaMargin { get; set; }
    public double NetDebtEbitda { get; set; }
}