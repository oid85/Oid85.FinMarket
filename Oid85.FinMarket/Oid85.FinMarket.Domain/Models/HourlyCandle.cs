namespace Oid85.FinMarket.Domain.Models;

/// <summary>
/// Часовая свеча
/// </summary>
public class HourlyCandle : DailyCandle
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