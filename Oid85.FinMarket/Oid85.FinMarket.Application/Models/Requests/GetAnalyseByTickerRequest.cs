namespace Oid85.FinMarket.Application.Models.Requests;

public class GetAnalyseByTickerRequest : GetAnalyseRequest
{
    public string Ticker { get; set; } = string.Empty;
}