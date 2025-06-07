namespace Oid85.FinMarket.Application.Models;

public class Trade
{
    public string Ticker { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public int CandleIndex { get; set; }
}