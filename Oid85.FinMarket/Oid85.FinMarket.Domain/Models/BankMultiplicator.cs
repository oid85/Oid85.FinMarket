namespace Oid85.FinMarket.Domain.Models;

public class BankMultiplicator
{
    public Guid Id { get; set; }
    public string Ticker { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double MarketCap { get; set; }
    public double NetIncome { get; set; }
    public double DdAo { get; set; }
    public double DdAp { get; set; }
    public double DdNetIncome { get; set; }
    public double Pe { get; set; }
    public double Pb { get; set; }
    public double NetOperatingIncome { get; set; }
    public double NetInterestMargin { get; set; }
    public double Roe { get; set; }
    public double Roa { get; set; }
}