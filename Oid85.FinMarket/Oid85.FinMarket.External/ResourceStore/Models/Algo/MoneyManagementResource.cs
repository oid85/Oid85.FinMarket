using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models.Algo;

/// <summary>
/// Настройки управления капиталом
/// </summary>
public class MoneyManagementResource
{
    /// <summary>
    /// Выделенный капитал
    /// </summary>
    [JsonPropertyName("money")]
    public double Money { get; set; }
    
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
    
    /// <summary>
    /// Выделенный капитал для стратегий статистического арбитража
    /// </summary>
    [JsonPropertyName("statisticalArbitrageMoney")]
    public double StatisticalArbitrageMoney { get; set; }   
    
    /// <summary>
    /// Плечо для акций для стратегий статистического арбитража
    /// </summary>
    [JsonPropertyName("statisticalArbitrageShareLeverage")]
    public double StatisticalArbitrageShareLeverage { get; set; }
    
    /// <summary>
    /// Плечо для фьючерсов для стратегий статистического арбитража
    /// </summary>
    [JsonPropertyName("statisticalArbitrageFutureLeverage")]
    public double StatisticalArbitrageFutureLeverage { get; set; }
}