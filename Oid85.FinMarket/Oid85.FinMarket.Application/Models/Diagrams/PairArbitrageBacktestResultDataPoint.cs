namespace Oid85.FinMarket.Application.Models.Diagrams;

public class PairArbitrageBacktestResultDataPoint
{
    public string? Date { get; set; } = null;
    public double? Price { get; set; } = null;
    public double Equity { get; set; } = 0.0;
    public double Drawdown { get; set; } = 0.0;
    public double? BuyPrice { get; set; } = null;
    public double? SellPrice { get; set; } = null;
}