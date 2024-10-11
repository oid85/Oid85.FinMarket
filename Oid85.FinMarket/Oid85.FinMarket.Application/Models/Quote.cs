using Skender.Stock.Indicators;

namespace Oid85.FinMarket.Application.Models
{
    public class Quote : IQuote
    {
        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public decimal Volume { get; set; }

        public DateTime Date { get; set; }
    }
}
