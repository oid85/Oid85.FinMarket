namespace Oid85.FinMarket.Application.Models.Requests
{
    public class GetReportAnalyseStockRequest
    {
        public DateTime From { get; set; } = DateTime.MinValue;
        public DateTime To { get; set; } = DateTime.MaxValue;
        public string Ticker { get; set; } = string.Empty;
    }
}
