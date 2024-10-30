using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class CandleEntity : BaseEntity
{
    /// <summary>
    /// Тикер
    /// </summary>
    [Column("ticker")]
    public string Ticker { get; set; } = string.Empty;
    
    /// <summary>
    /// Таймфрейм
    /// </summary>
    [Column("timeframe")]
    public string Timeframe { get; set; } = string.Empty;
    
    /// <summary>
    /// Цена открытия
    /// </summary>
    [Column("open")]
    public double Open { get; set; }

    /// <summary>
    /// Цена закрытия
    /// </summary>
    [Column("close")]
    public double Close { get; set; }

    /// <summary>
    /// Макс. цена
    /// </summary>
    [Column("high")]
    public double High { get; set; }

    /// <summary>
    /// Мин. цена
    /// </summary>
    [Column("low")]
    public double Low { get; set; }

    /// <summary>
    /// Объем
    /// </summary>
    [Column("volume")]
    public long Volume { get; set; }
    
    /// <summary>
    /// Время
    /// </summary>
    [Column("date", TypeName = "timestamp with time zone")]
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Свеча сформирована
    /// </summary>
    [Column("is_complete")]
    public bool IsComplete { get; set; }
}