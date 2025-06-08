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
    public RiskLevelsResource RiskLevelsResource { get; set; } = new();
    
    [JsonPropertyName("yield")]
    public RangeResource<double> Yield { get; set; } = new();
    
    [JsonPropertyName("yearsToMaturity")]
    public RangeResource<int> YearsToMaturity { get; set; } = new();
    
    [JsonPropertyName("price")]
    public RangeResource<double> Price { get; set; } = new();
}