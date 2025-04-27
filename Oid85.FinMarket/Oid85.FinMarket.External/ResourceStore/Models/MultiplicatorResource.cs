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
    public MultiplicatorUnitResource<double> TotalSharesAo { get; set; } = new();
    
    /// <summary>
    /// Количество акций привелегированных
    /// </summary>
    [JsonPropertyName("totalSharesAp")]
    public MultiplicatorUnitResource<double> TotalSharesAp { get; set; } = new();
    
    /// <summary>
    /// Тикер, ао
    /// </summary>
    [JsonPropertyName("tickerAo")]
    public ValueUnitResource<string> TickerAo { get; set; } = new();
    
    /// <summary>
    /// Тикер, ап
    /// </summary>
    [JsonPropertyName("tickerAp")]
    public ValueUnitResource<string> TickerAp { get; set; } = new();
    
    /// <summary>
    /// Бета-коэффициент
    /// </summary>
    [JsonPropertyName("beta")]
    public ValueUnitResource<double> Beta { get; set; } = new();
    
    /// <summary>
    /// Выручка
    /// </summary>
    [JsonPropertyName("revenue")]
    public MultiplicatorUnitResource<double> Revenue { get; set; } = new();
    
    /// <summary>
    /// Чистая прибыль
    /// </summary>
    [JsonPropertyName("netIncome")]
    public MultiplicatorUnitResource<double> NetIncome { get; set; } = new();
    
    /// <summary>
    /// Операционная прибыль
    /// </summary>
    [JsonPropertyName("operatingIncome")]
    public MultiplicatorUnitResource<double> OperatingIncome { get; set; } = new();
    
    /// <summary>
    /// EBITDA
    /// </summary>
    [JsonPropertyName("ebitda")]
    public MultiplicatorUnitResource<double> Ebitda { get; set; } = new();
    
    /// <summary>
    /// P/E
    /// </summary>
    [JsonPropertyName("pe")]
    public MultiplicatorUnitResource<double> Pe { get; set; } = new();
    
    /// <summary>
    /// P/B
    /// </summary>
    [JsonPropertyName("pb")]
    public MultiplicatorUnitResource<double> Pb { get; set; } = new();
    
    /// <summary>
    /// P/BV
    /// </summary>
    [JsonPropertyName("pbv")]
    public MultiplicatorUnitResource<double> Pbv { get; set; } = new();
    
    /// <summary>
    /// EV
    /// </summary>
    [JsonPropertyName("ev")]
    public MultiplicatorUnitResource<double> Ev { get; set; } = new();
    
    /// <summary>
    /// BV
    /// </summary>
    [JsonPropertyName("bv")]
    public MultiplicatorUnitResource<double> Bv { get; set; } = new();    
    
    /// <summary>
    /// ROE
    /// </summary>
    [JsonPropertyName("roe")]
    public MultiplicatorUnitResource<double> Roe { get; set; } = new();
    
    /// <summary>
    /// ROA
    /// </summary>
    [JsonPropertyName("roa")]
    public MultiplicatorUnitResource<double> Roa { get; set; } = new();
    
    /// <summary>
    /// EPS
    /// </summary>
    [JsonPropertyName("eps")]
    public MultiplicatorUnitResource<double> Eps { get; set; } = new();
    
    /// <summary>
    /// Чистая процентная маржа
    /// </summary>
    [JsonPropertyName("netInterestMargin")]
    public MultiplicatorUnitResource<double> NetInterestMargin { get; set; } = new();
    
    /// <summary>
    /// Общий долг
    /// </summary>
    [JsonPropertyName("totalDebt")]
    public MultiplicatorUnitResource<double> TotalDebt { get; set; } = new();

    /// <summary>
    /// Чистый долг
    /// </summary>
    [JsonPropertyName("netDebt")]
    public MultiplicatorUnitResource<double> NetDebt { get; set; } = new();
}
