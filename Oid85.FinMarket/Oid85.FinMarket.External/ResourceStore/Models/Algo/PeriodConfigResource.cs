using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models.Algo;

/// <summary>
/// Настройки периодов бэктеста и оптимизации
/// </summary>
public class PeriodConfigResource
{
    /// <summary>
    /// Период стабилизации в свечах
    /// </summary>
    [JsonPropertyName("stabilizationPeriodInCandles")]
    public int StabilizationPeriodInCandles { get; set; }
    
    /// <summary>
    /// Размер окна оптимизации в днях
    /// </summary>
    [JsonPropertyName("optimizationWindowInDays")]
    public int OptimizationWindowInDays { get; set; }
    
    /// <summary>
    /// Размер окна бэктеста в днях
    /// </summary>
    [JsonPropertyName("backtestWindowInDays")]
    public int BacktestWindowInDays { get; set; }
    
    /// <summary>
    /// Смещение окна бэктеста относительно окна оптимизации
    /// </summary>
    [JsonPropertyName("backtestShiftInDays")]
    public int BacktestShiftInDays { get; set; }
    
    /// <summary>
    /// Период стабилизации в днях для дневного таймфрейма
    /// </summary>
    [JsonPropertyName("dailyStabilizationPeriodInDays")]
    public int DailyStabilizationPeriodInDays { get; set; }
    
    /// <summary>
    /// Период стабилизации в днях для часового таймфрейма
    /// </summary>
    [JsonPropertyName("hourlyStabilizationPeriodInDays")]
    public int HourlyStabilizationPeriodInDays { get; set; }
    
    /// <summary>
    /// Период расчета хвостов регрессии в днях
    /// </summary>
    [JsonPropertyName("calculateRegressionTailsPeriodInDays")]
    public int CalculateRegressionTailsPeriodInDays { get; set; }
}