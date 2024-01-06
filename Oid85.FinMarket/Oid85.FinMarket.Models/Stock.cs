namespace Oid85.FinMarket.Models;

public class Stock
{
    public long Id { get; set; }
    
    public string Ticker { get; set; }
    
    public string Name { get; set; }
    
    public string Figi { get; set; }
    
    public bool InWatchList { get; set; }
}