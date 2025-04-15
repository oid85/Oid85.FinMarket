using System.Text.Json.Serialization;

namespace Oid85.FinMarket.Application.Models.Requests;

public class DateRangeRequest
{
    [JsonPropertyName("from")]
    public DateOnly From { get; set; } = DateOnly.MinValue;
    
    [JsonPropertyName("to")]
    public DateOnly To { get; set; } = DateOnly.MaxValue;
    
    [JsonPropertyName("tickerList")]
    public string TickerList { get; set; } = string.Empty;
}