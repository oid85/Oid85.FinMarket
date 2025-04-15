using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Список тикеров
/// </summary>
public class TickerListResource
{
    /// <summary>
    /// Наименование
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Описание
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Список тикеров
    /// </summary>
    [JsonPropertyName("tickers")]
    public List<string> Tickers { get; set; } = [];
}