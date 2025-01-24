using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Значение - единица измерения
/// </summary>
public class ValueUnitResource<T>
{
    /// <summary>
    /// Значение
    /// </summary>
    [JsonPropertyName("value")]
    public T? Value { get; set; } = default;
    
    /// <summary>
    /// Единица измерения
    /// </summary>
    [JsonPropertyName("unit")]
    public string Unit { get; set; } = string.Empty;
}

