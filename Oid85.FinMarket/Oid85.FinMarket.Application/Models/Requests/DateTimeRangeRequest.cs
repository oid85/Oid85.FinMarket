using System.Text.Json.Serialization;

namespace Oid85.FinMarket.Application.Models.Requests;

public class DateTimeRangeRequest
{
    [JsonPropertyName("from")]
    public DateTime From { get; set; } = DateTime.MinValue;
    
    [JsonPropertyName("to")]
    public DateTime To { get; set; } = DateTime.MaxValue;
}