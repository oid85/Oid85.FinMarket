using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Цвет для значения
/// </summary>
public class ValueColorResource<T>
{
    /// <summary>
    /// Значение
    /// </summary>
    [JsonPropertyName("value")]
    public T? Value { get; set; }
    
    /// <summary>
    /// Код цвета (RGB)
    /// </summary>
    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;
}
