namespace Oid85.FinMarket.Application.Models.Diagrams;

public class PairArbitrageBacktestResultDataPoint
{
    public string? Date { get; set; } = null;
    public double? PriceFirst { get; set; } = null;
    public double? PriceSecond { get; set; } = null;
    public double? Spread { get; set; } = null;
    public double Equity { get; set; } = 0.0;
    public double Drawdown { get; set; } = 0.0;
    public double? BuyPriceFirst { get; set; } = null;
    public double? SellPriceFirst { get; set; } = null;
    public double? BuyPriceSecond { get; set; } = null;
    public double? SellPriceSecond { get; set; } = null;
}