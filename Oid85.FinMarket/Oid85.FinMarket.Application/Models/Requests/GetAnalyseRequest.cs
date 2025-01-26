using System.Text.Json.Serialization;

namespace Oid85.FinMarket.Application.Models.Requests;

public class GetAnalyseRequest
{
    [JsonPropertyName("from")]
    public DateOnly From { get; set; } = DateOnly.MinValue;
    
    [JsonPropertyName("to")]
    public DateOnly To { get; set; } = DateOnly.MaxValue;
}