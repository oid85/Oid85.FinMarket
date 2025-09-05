using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models.Algo;

/// <summary>
/// Общие настройки Алго
/// </summary>
public class AlgoConfigResource
{
    /// <summary>
    /// Настройки периодов бэктеста и оптимизации
    /// </summary>
    [JsonPropertyName("periodConfig")]
    public PeriodConfigResource PeriodConfigResource { get; set; } = new();    
    
    /// <summary>
    /// Настройки управления капиталом
    /// </summary>
    [JsonPropertyName("moneyManagement")]
    public MoneyManagementResource MoneyManagementResource { get; set; } = new();   
    
    /// <summary>
    /// Фильтр результатов оптимизации
    /// </summary>
    [JsonPropertyName("optimizationResultFilter")]
    public OptimizationResultFilterResource OptimizationResultFilterResource { get; set; } = new();
    
    /// <summary>
    /// Фильтр результатов бэктеста
    /// </summary>
    [JsonPropertyName("backtestResultFilter")]
    public BacktestResultFilterResource BacktestResultFilterResource { get; set; } = new();
    
    /// <summary>
    /// Фильтр результатов оптимизации
    /// </summary>
    [JsonPropertyName("pairArbitrageOptimizationResultFilter")]
    public PairArbitrageOptimizationResultFilterResource PairArbitrageOptimizationResultFilterResource { get; set; } = new();
    
    /// <summary>
    /// Фильтр результатов бэктеста
    /// </summary>
    [JsonPropertyName("pairArbitrageBacktestResultFilter")]
    public PairArbitrageBacktestResultFilterResource PairArbitrageBacktestResultFilterResource { get; set; } = new();
}