namespace Oid85.FinMarket.Application.Models.Diagrams;

public class BacktestResultDataPoint
{
    public string? Date { get; set; } = null;
    public double?[] ChannelBands { get; set; } = [null, null];
    public double? Indicator { get; set; } = null;
    public double? Filter { get; set; } = null;
    public double? Price { get; set; } = null;
    public double Equity { get; set; } = 0.0;
    public double Drawdown { get; set; } = 0.0;
    public double? BuyPrice { get; set; } = null;
    public double? SellPrice { get; set; } = null;
}