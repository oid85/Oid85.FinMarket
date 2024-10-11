namespace Oid85.FinMarket.Domain.Models
{
    public class MoexIndexItem
    {
        public long Id { get; set; }
        public string Ticker { get; set; } = string.Empty;
        public long NumberShares { get; set; }
        public double Price { get; set; }
        public double Prc { get; set; }
    }
}
