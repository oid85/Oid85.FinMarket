using System.Text.Json.Serialization;

namespace Oid85.FinMarket.Application.Models.Requests;

public class TickerListRequest
{
    [JsonPropertyName("tickerList")]
    public string TickerList { get; set; } = string.Empty;
}