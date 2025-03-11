using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Флаг вкл/откл по имени
/// </summary>
public class EnableNameResource
{
    /// <summary>
    /// Включен
    /// </summary>
    [JsonPropertyName("enable")]
    public bool Enable { get; set; }
    
    /// <summary>
    /// Наименвание
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

