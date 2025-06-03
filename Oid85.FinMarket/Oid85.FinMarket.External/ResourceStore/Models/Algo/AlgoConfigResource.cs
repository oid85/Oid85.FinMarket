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
    public PeriodConfig PeriodConfig { get; set; } = new();    
    
    /// <summary>
    /// Настройки управления капиталом
    /// </summary>
    [JsonPropertyName("moneyManagement")]
    public MoneyManagement MoneyManagement { get; set; } = new();   
    
    /// <summary>
    /// Фильтр результатов оптимизации
    /// </summary>
    [JsonPropertyName("optimizationResultFilter")]
    public OptimizationResultFilter OptimizationResultFilter { get; set; } = new();
    
    /// <summary>
    /// Фильтр результатов бэктеста
    /// </summary>
    [JsonPropertyName("backtestResultFilter")]
    public BacktestResultFilter BacktestResultFilter { get; set; } = new();
}