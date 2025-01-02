namespace Oid85.FinMarket.Application.Models.Requests;

public class GetReportAnalyseByTickerRequest : GetReportAnalyseRequest
{
    public string Ticker { get; set; } = string.Empty;
}