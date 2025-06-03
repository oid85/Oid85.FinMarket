using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models.Algo;

/// <summary>
/// Настройки стратегии
/// </summary>
public class AlgoStrategyResource
{
    /// <summary>
    /// Идентификатор стратегии
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Флаг включена/выключена
    /// </summary>
    [JsonPropertyName("enable")]
    public bool Enable { get; set; }    
    
    /// <summary>
    ///  Наименование стратегии
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    ///  Таймфрейм стратегии
    /// </summary>
    [JsonPropertyName("timeframe")]
    public string Timeframe { get; set; } = string.Empty;    
    
    /// <summary>
    /// Параметры стратегии
    /// </summary>
    [JsonPropertyName("params")]
    public List<StrategyParam> Params { get; set; } = new();
}