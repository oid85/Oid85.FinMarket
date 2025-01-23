using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Мультипликатор
/// </summary>
public class MultiplicatorResource
{
    /// <summary>
    /// Количество акций обыкновенных
    /// </summary>
    [JsonPropertyName("totalSharesAo")]
    public ValueUnitResource<double> TotalSharesAo { get; set; } = new();
    
    /// <summary>
    /// Количество акций привелегированных
    /// </summary>
    [JsonPropertyName("totalSharesAp")]
    public ValueUnitResource<double> TotalSharesAp { get; set; } = new();
    
    /// <summary>
    /// Выручка
    /// </summary>
    [JsonPropertyName("revenue")]
    public ValueUnitResource<double> Revenue { get; set; } = new();
    
    /// <summary>
    /// Чистая прибыль
    /// </summary>
    [JsonPropertyName("netIncome")]
    public ValueUnitResource<double> NetIncome { get; set; } = new();
    
    /// <summary>
    /// Операционная прибыль
    /// </summary>
    [JsonPropertyName("operatingIncome")]
    public ValueUnitResource<double> OperatingIncome { get; set; } = new();
    
    /// <summary>
    /// EBITDA
    /// </summary>
    [JsonPropertyName("ebitda")]
    public ValueUnitResource<double> Ebitda { get; set; } = new();
    
    /// <summary>
    /// P/E
    /// </summary>
    [JsonPropertyName("pe")]
    public ValueUnitResource<double> Pe { get; set; } = new();
    
    /// <summary>
    /// P/B
    /// </summary>
    [JsonPropertyName("pb")]
    public ValueUnitResource<double> Pb { get; set; } = new();
    
    /// <summary>
    /// P/BV
    /// </summary>
    [JsonPropertyName("pbv")]
    public ValueUnitResource<double> Pbv { get; set; } = new();
    
    /// <summary>
    /// EV
    /// </summary>
    [JsonPropertyName("ev")]
    public ValueUnitResource<double> Ev { get; set; } = new();
    
    /// <summary>
    /// ROE
    /// </summary>
    [JsonPropertyName("roe")]
    public ValueUnitResource<double> Roe { get; set; } = new();
    
    /// <summary>
    /// ROA
    /// </summary>
    [JsonPropertyName("roa")]
    public ValueUnitResource<double> Roa { get; set; } = new();
    
    /// <summary>
    /// EPS
    /// </summary>
    [JsonPropertyName("eps")]
    public ValueUnitResource<double> Eps { get; set; } = new();
    
    /// <summary>
    /// Чистая процентная маржа
    /// </summary>
    [JsonPropertyName("netInterestMargin")]
    public ValueUnitResource<double> NetInterestMargin { get; set; } = new();
    
    /// <summary>
    /// Общий долг
    /// </summary>
    [JsonPropertyName("totalDebt")]
    public ValueUnitResource<double> TotalDebt { get; set; } = new();

    /// <summary>
    /// Чистый долг
    /// </summary>
    [JsonPropertyName("netDebt")]
    public ValueUnitResource<double> NetDebt { get; set; } = new();
}
