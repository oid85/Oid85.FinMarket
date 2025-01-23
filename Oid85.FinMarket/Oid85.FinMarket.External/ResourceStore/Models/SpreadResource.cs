using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Спред
/// </summary>
public class SpreadResource
{
    /// <summary>
    /// Тикер базового актива
    /// </summary>
    [JsonPropertyName("baseAssetTicker")]
    public string BaseAssetTicker { get; set; } = string.Empty;
    
    /// <summary>
    /// Тикер вечного фьючерса
    /// </summary>
    [JsonPropertyName("foreverFutureTicker")]
    public string ForeverFutureTicker { get; set; } = string.Empty;
    
    /// <summary>
    /// Префикс тикера фьючерса
    /// </summary>
    [JsonPropertyName("futureTickerPrefix")]
    public string FutureTickerPrefix { get; set; } = string.Empty;

    /// <summary>
    /// Множитель
    /// </summary>
    [JsonPropertyName("multiplier")]
    public double Multiplier { get; set; } = 1.0;
}

