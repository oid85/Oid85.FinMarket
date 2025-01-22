namespace Oid85.FinMarket.Application.Models.Resources;

/// <summary>
/// Спред
/// </summary>
public class SpreadResource
{
    /// <summary>
    /// Тикер базового актива
    /// </summary>
    public string BaseAssetTicker { get; set; } = string.Empty;
    
    /// <summary>
    /// Тикер вечного фьючерса
    /// </summary>
    public string ForeverFutureTicker { get; set; } = string.Empty;
    
    /// <summary>
    /// Префикс тикера фьючерса
    /// </summary>
    public string FutureTickerPrefix { get; set; } = string.Empty;

    /// <summary>
    /// Множитель
    /// </summary>
    public double Multiplier { get; set; } = 1.0;
}

