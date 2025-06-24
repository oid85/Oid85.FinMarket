using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models.Algo;

/// <summary>
/// Фильтр результатов бэктеста
/// </summary>
public class BacktestResultFilterResource
{
    /// <summary>
    /// Минимальный критерий по фактору прибыли
    /// </summary>
    [JsonPropertyName("minProfitFactor")]
    public double MinProfitFactor { get; set; }
    
    /// <summary>
    ///  Минимальный критерий по фактору восстановления
    /// </summary>
    [JsonPropertyName("minRecoveryFactor")]
    public double MinRecoveryFactor { get; set; }
    
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
    
    /// <summary>
    /// Критерий по максимальной просадке, %
    /// </summary>
    [JsonPropertyName("maxDrawdownPercent")]
    public double MaxDrawdownPercent { get; set; }
}