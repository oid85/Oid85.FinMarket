using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class FutureEntity : AuditableEntity
{
    /// <summary>
    /// Тикер
    /// </summary>
    [Column("ticker"), MaxLength(20)]
    public string Ticker { get; set; } = string.Empty;

    /// <summary>
    /// Цена инструмента
    /// </summary>
    [Column("price")]
    public double Price { get; set; }

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
    [Column("name"), MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата истечения срока
    /// </summary>
    [Column("expiration_date", TypeName = "date")]
    public DateOnly ExpirationDate { get; set; } = DateOnly.MinValue;
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    [Column("in_watch_list")]
    public bool InWatchList { get; set; } = false; 
}