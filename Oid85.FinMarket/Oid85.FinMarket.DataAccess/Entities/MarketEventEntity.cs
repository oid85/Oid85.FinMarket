using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class MarketEventEntity : AuditableEntity
{
    /// <summary>
    /// Дата
    /// </summary>
    [Column("date", TypeName = "date")]
    public DateOnly Date { get; set; } 
    
    /// <summary>
    /// Дата
    /// </summary>
    [Column("time", TypeName = "time")]
    public TimeOnly Time { get; set; } 
    
    /// <summary>
    /// Тикер
    /// </summary>
    [Column("ticker"), MaxLength(20)]
    public string Ticker { get; set; } = string.Empty;
    
    /// <summary>
    /// Наименование инструмента
    /// </summary>
    [Column("instrument_name"), MaxLength(200)]
    public string InstrumentName { get; set; } = string.Empty;
    
    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    [Column("instrument_id")]
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Тип события
    /// </summary>
    [Column("market_event_type"), MaxLength(100)]
    public string MarketEventType { get; set; } = string.Empty;
    
    /// <summary>
    /// Техт
    /// </summary>
    [Column("market_event_text"), MaxLength(1000)]
    public string MarketEventText { get; set; } = string.Empty;
    
    /// <summary>
    /// Активно/неактивно
    /// </summary>
    [Column("is_active")]
    public bool IsActive { get; set; } = false;
    
    /// <summary>
    /// Отправлено оповещение 
    /// </summary>
    [Column("sent_notification")]
    public bool SentNotification { get; set; } = false;
}