namespace Oid85.FinMarket.Domain.Models.Algo;

public class ArbitragePosition
{
    public (string First, string Second) Ticker { get; set; }
    public (double First, double Second) EntryPrice { get; set; }
    public (double First, double Second) ExitPrice { get; set; }
    public DateTime EntryDateTime { get; set; }
    public DateTime ExitDateTime { get; set; }
    public int EntryCandleIndex { get; set; }
    public int ExitCandleIndex { get; set; }
    public bool IsActive { get; set; }
    public bool IsLongShort { get; set; }
    public bool IsShortLong { get; set; }
    public (int First, int Second) Quantity { get; set; }
    public double Cost { get; set; }
    public double Profit { get; set; }
    public double ProfitPercent { get; set; }
    public double TotalProfit { get; set; }
    public double TotalProfitPct { get; set; }
}