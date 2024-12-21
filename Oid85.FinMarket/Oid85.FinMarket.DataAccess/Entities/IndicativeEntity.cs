using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class IndicativeEntity : AuditableEntity
{
    /// <summary>
    /// FIGI-идентификатор инструмента
    /// </summary>
    [Column("figi")]
    public string Figi { get; set; } = string.Empty;
    
    /// <summary>
    /// Тикер инструмента
    /// </summary>
    [Column("ticker")]
    public string Ticker { get; set; } = string.Empty;
    
    /// <summary>
    /// Цена инструмента
    /// </summary>
    [Column("price")]
    public double Price { get; set; }
    
    /// <summary>
    /// Класс-код инструмента
    /// </summary>
    [Column("class_code")]
    public string ClassCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Валюта расчётов
    /// </summary>
    [Column("currency")]
    public string Currency { get; set; } = string.Empty;
    
    /// <summary>
    /// Тип инструмента
    /// </summary>
    [Column("instrument_kind")]
    public string InstrumentKind { get; set; } = string.Empty;
    
    /// <summary>
    /// Название инструмента
    /// </summary>
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Tорговая площадка (секция биржи)
    /// </summary>
    [Column("exchange")]
    public string Exchange { get; set; } = string.Empty;
    
    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    [Column("uid")]
    public string Uid { get; set; } = string.Empty;
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    [Column("in_watch_list")]
    public bool InWatchList { get; set; } = false; 
}