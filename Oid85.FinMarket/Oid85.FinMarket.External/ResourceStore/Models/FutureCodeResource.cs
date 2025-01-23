using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Код фьючерса (месячный)
/// </summary>
public class FutureCodeResource
{
    /// <summary>
    /// Суффикс
    /// </summary>
    [JsonPropertyName("suffix")]
    public string Suffix { get; set; } = string.Empty;
    
    /// <summary>
    /// Месяц
    /// </summary>
    [JsonPropertyName("month")]
    public string Month { get; set; } = string.Empty;
}

