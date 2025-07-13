using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models.Algo;

/// <summary>
/// Настройки управления капиталом
/// </summary>
public class MoneyManagementResource
{
    /// <summary>
    /// Выделенный стратегии капитал
    /// </summary>
    [JsonPropertyName("money")]
    public double Money { get; set; }
    
    /// <summary>
    /// Процент от капитала для входа в сделку
    /// </summary>
    [JsonPropertyName("percentOfMoney")]
    public double PercentOfMoney { get; set; }
    
    /// <summary>
    /// Размер юнита, руб
    /// </summary>
    [JsonPropertyName("unitSize")]
    public double UnitSize { get; set; }
    
    /// <summary>
    /// Плечо для акций
    /// </summary>
    [JsonPropertyName("shareLeverage")]
    public double ShareLeverage { get; set; }
    
    /// <summary>
    /// Плечо для фьючерсов
    /// </summary>
    [JsonPropertyName("futureLeverage")]
    public double FutureLeverage { get; set; }
}