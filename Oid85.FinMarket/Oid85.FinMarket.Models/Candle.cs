namespace Oid85.FinMarket.Models;

public class Candle
{
    public long Id { get; set; }
    
    public DateTime DateTime { get; set; }

    public double Open { get; set; }

    public double Close { get; set; }

    public double High { get; set; }

    public double Low { get; set; }

    public long Volume { get; set; }
    
    public string Ticker { get; set; }
}