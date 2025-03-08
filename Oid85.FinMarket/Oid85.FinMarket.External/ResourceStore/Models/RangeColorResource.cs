using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Цвет диапазона
/// </summary>
public class RangeColorResource
{
    /// <summary>
    /// Верхний уровень
    /// </summary>
    [JsonPropertyName("highLevel")]
    public double HighLevel { get; set; }
    
    /// <summary>
    /// Нижний уровень
    /// </summary>
    [JsonPropertyName("lowLevel")]
    public double LowLevel { get; set; }
    
    /// <summary>
    /// Код цвета (RGB)
    /// </summary>
    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;
}
