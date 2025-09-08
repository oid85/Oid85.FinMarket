using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models.Algo;

/// <summary>
/// Фильтр результатов оптимизации
/// </summary>
public class PairArbitrageOptimizationResultFilterResource
{
    /// <summary>
    /// Минимальный критерий по проценту выигрышных сделок, %
    /// </summary>
    [JsonPropertyName("minWinningTradesPercent")]
    public double MinWinningTradesPercent { get; set; }
    
    /// <summary>
    /// Максимальный критерий по проценту выигрышных сделок, %
    /// </summary>
    [JsonPropertyName("maxWinningTradesPercent")]
    public double MaxWinningTradesPercent { get; set; }
    
    /// <summary>
    /// Минимальный критерий по годовой прибыли, %
    /// </summary>
    [JsonPropertyName("minAnnualYieldReturn")]
    public double MinAnnualYieldReturn { get; set; }
}