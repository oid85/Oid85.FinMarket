using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Дата - значение
/// </summary>
public class DateValueResource<T>
{
    /// <summary>
    /// Дата
    /// </summary>
    [JsonPropertyName("date")]
    public DateOnly Date { get; set; } = DateOnly.MinValue;
    
    /// <summary>
    /// Значение
    /// </summary>
    [JsonPropertyName("value")]
    public T? Value { get; set; } = default;
}

