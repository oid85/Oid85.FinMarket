using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class FinIndexEntity : AuditableEntity
{
    /// <summary>
    /// FIGI-идентификатор инструмента
    /// </summary>
    [Column("figi"), MaxLength(20)]
    public string Figi { get; set; } = string.Empty;
    
    /// <summary>
    /// Тикер инструмента
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
    /// Класс-код инструмента
    /// </summary>
    [Column("class_code"), MaxLength(20)]
    public string ClassCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Валюта расчётов
    /// </summary>
    [Column("currency"), MaxLength(20)]
    public string Currency { get; set; } = string.Empty;
    
    /// <summary>
    /// Тип инструмента
    /// </summary>
    [Column("instrument_kind"), MaxLength(20)]
    public string InstrumentKind { get; set; } = string.Empty;
    
    /// <summary>
    /// Название инструмента
    /// </summary>
    [Column("name"), MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Tорговая площадка (секция биржи)
    /// </summary>
    [Column("exchange"), MaxLength(20)]
    public string Exchange { get; set; } = string.Empty;
    
    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    [Column("instrument_id")]
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    [Column("in_watch_list")]
    public bool InWatchList { get; set; } = false; 
}