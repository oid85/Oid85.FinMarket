using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models;

public class RiskLevels
{
    [JsonPropertyName("low")]
    public bool Low { get; set; }
    
    [JsonPropertyName("middle")]
    public bool Middle { get; set; }
    
    [JsonPropertyName("high")]
    public bool High { get; set; }
}