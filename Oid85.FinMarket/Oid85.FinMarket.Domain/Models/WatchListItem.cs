namespace Oid85.FinMarket.Domain.Models
{
    public class WatchListItem
    {
        public long Id { get; set; }
        public string Ticker { get; set; } = string.Empty;
        public double Price { get; set; }
    }
}
