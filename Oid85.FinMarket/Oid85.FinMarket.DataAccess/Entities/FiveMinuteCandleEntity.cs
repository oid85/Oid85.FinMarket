using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class FiveMinuteCandleEntity : BaseEntity
{
    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    [Column("instrument_id")]
    public Guid InstrumentId { get; set; }
    
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
    /// Дата
    /// </summary>
    [Column("date", TypeName = "date")]
    public DateOnly Date { get; set; }
    
    /// <summary>
    /// Время
    /// </summary>
    [Column("time", TypeName = "time")]
    public TimeOnly Time { get; set; }
    
    
    /// <summary>
    /// Метка времени
    /// </summary>
    [Column("datetime", TypeName = "timestamp with time zone")]
    public DateTime DateTime { get; set; }
    
    /// <summary>
    /// Свеча сформирована
    /// </summary>
    [Column("is_complete")]
    public bool IsComplete { get; set; }
}