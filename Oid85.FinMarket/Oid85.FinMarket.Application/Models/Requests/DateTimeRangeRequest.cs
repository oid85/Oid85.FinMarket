using System.Text.Json.Serialization;

namespace Oid85.FinMarket.Application.Models.Requests;

public class DateTimeRangeRequest
{
    [JsonPropertyName("from")]
    public string From { get; set; } = string.Empty;
    
    [JsonPropertyName("to")]
    public string To { get; set; } = string.Empty;
}