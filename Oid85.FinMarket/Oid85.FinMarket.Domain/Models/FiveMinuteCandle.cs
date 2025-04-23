namespace Oid85.FinMarket.Domain.Models;

/// <summary>
/// 5-минутная свеча
/// </summary>
public class FiveMinuteCandle : Candle
{
    /// <summary>
    /// Время
    /// </summary>
    public TimeOnly Time { get; set; }
    
    /// <summary>
    /// Метка времени
    /// </summary>
    public long DateTimeTicks { get; set; }
}