using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class ShareEntity : AuditableEntity
{
    /// <summary>
    /// Тикер
    /// </summary>
    [Column("ticker"), MaxLength(20)]
    public string Ticker { get; set; } = string.Empty;

    /// <summary>
    /// Цена инструмента
    /// </summary>
    [Column("last_price")]
    public double LastPrice { get; set; }
    
    /// <summary>
    /// Нижний целевой уровень
    /// </summary>
    [Column("high_target_price")]
    public double HighTargetPrice { get; set; }
    
    /// <summary>
    /// Верхний целевой уровень
    /// </summary>
    [Column("low_target_price")]
    public double LowTargetPrice { get; set; }
    
    /// <summary>
    /// Идентификатор ISIN
    /// </summary>
    [Column("isin"), MaxLength(20)]
    public string Isin { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор FIGI
    /// </summary>
    [Column("figi"), MaxLength(20)]
    public string Figi { get; set; } = string.Empty;

    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    [Column("instrument_id")]
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    [Column("name"), MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Сектор
    /// </summary>
    [Column("sector"), MaxLength(20)]
    public string Sector { get; set; } = string.Empty;
}