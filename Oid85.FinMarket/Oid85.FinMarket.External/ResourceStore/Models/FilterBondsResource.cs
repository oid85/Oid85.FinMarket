using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Фильтр облигаций
/// </summary>
public class FilterBondsResource
{
    [JsonPropertyName("sectors")]
    public List<string> Sectors { get; set; } = new();
    
    [JsonPropertyName("currencies")]
    public List<string> Currencies { get; set; } = new();
    
    [JsonPropertyName("riskLevels")]
    public RiskLevels RiskLevels { get; set; } = new();
    
    [JsonPropertyName("yield")]
    public Range<double> Yield { get; set; } = new();
    
    [JsonPropertyName("yearsToMaturity")]
    public Range<int> YearsToMaturity { get; set; } = new();
    
    [JsonPropertyName("price")]
    public Range<double> Price { get; set; } = new();
}