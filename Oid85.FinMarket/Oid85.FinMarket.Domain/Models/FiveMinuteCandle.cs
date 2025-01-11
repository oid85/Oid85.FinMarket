namespace Oid85.FinMarket.Domain.Models;

/// <summary>
/// 5-минутная свеча
/// </summary>
public class FiveMinuteCandle : Candle
{
    /// <summary>
    /// Дата
    /// </summary>
    public TimeOnly Time { get; set; }
}