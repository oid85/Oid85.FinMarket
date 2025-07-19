using System.Text.Json.Serialization;

namespace Oid85.FinMarket.Application.Models.Requests;

public class TickerStrategyRequest
{
    [JsonPropertyName("ticker")]
    public string Ticker { get; set; } = string.Empty;
    
    [JsonPropertyName("strategyName")]
    public string StrategyName { get; set; } = string.Empty;
}