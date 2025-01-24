using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Ценовой уровень
/// </summary>
public class PriceLevelResource
{
    /// <summary>
    /// Уровень включен
    /// </summary>
    [JsonPropertyName("enable")]
    public bool Enable { get; set; }
    
    /// <summary>
    /// Наименвание
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Значение
    /// </summary>
    [JsonPropertyName("value")]
    public double Value { get; set; }
}

