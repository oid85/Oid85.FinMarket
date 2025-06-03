using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models.Algo;

/// <summary>
/// Настройки управления капиталом
/// </summary>
public class MoneyManagement
{
    /// <summary>
    /// Выделенный стратегии капитал
    /// </summary>
    [JsonPropertyName("money")]
    public double Money { get; set; }
    
    /// <summary>
    /// Процент от капитала для входа в сделку
    /// </summary>
    [JsonPropertyName("percentSize")]
    public double PercentSize { get; set; }
}