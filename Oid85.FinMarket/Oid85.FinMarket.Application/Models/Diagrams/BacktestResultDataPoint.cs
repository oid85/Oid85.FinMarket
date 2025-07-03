namespace Oid85.FinMarket.Application.Models.Diagrams;

public class BacktestResultDataPoint
{
    public string? Date { get; set; } = null;
    public double?[] ChannelBands { get; set; } = [null, null];
    public double? Indicator1 { get; set; } = null;
    public double? Indicator2 { get; set; } = null;
    public double? Indicator3 { get; set; } = null;
    public double? Price { get; set; } = null;
    public double? Equity { get; set; } = null;
    public double? Drawdown { get; set; } = null;
    public double? BuyPrice { get; set; } = null;
    public double? SellPrice { get; set; } = null;
}