using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models.Algo;

/// <summary>
/// Параметр оптимизации стратегии
/// </summary>
public class StrategyParamResource
{
    /// <summary>
    /// Наименование параметра
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Значение по-умолчанию
    /// </summary>
    [JsonPropertyName("default")]
    public int Default { get; set; }
    
    /// <summary>
    /// Минимальное значение
    /// </summary>
    [JsonPropertyName("min")]
    public int Min { get; set; }
    
    /// <summary>
    /// Максимальное значение
    /// </summary>
    [JsonPropertyName("max")]
    public int Max { get; set; }
    
    /// <summary>
    /// Шаг
    /// </summary>
    [JsonPropertyName("step")]
    public int Step { get; set; }
}