namespace Oid85.FinMarket.Models;

public class DownloadRequest
{
    public string Figi { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string Timeframe { get; set; }
}