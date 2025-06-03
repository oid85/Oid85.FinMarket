using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models.Algo;

/// <summary>
/// Фильтр результатов оптимизации
/// </summary>
public class OptimizationResultFilter
{
    /// <summary>
    /// Критерий по фактору прибыли
    /// </summary>
    [JsonPropertyName("profitFactor")]
    public double ProfitFactor { get; set; }
    
    /// <summary>
    ///  Критерий по фактору восстановления
    /// </summary>
    [JsonPropertyName("recoveryFactor")]
    public double RecoveryFactor { get; set; }
    
    /// <summary>
    /// Критерий по максимальной просадке, %
    /// </summary>
    [JsonPropertyName("maxDrawdownPercent")]
    public double MaxDrawdownPercent { get; set; }
}