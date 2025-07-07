using System.Text.Json.Serialization;

namespace Oid85.FinMarket.Application.Models.Requests;

public class TickerRequest
{
    [JsonPropertyName("ticker")]
    public string Ticker { get; set; }
}