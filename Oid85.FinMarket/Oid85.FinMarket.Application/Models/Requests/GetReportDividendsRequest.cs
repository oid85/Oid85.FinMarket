using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Models.Requests
{
    public class GetReportDividendsRequest
    {
        public string TickerList { get; set; } = KnownTickerLists.WatchListStocks;
    }
}
