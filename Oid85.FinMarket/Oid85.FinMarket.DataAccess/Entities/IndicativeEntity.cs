using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class IndicativeEntity : AuditableEntity
{
    /// <summary>
    /// FIGI-идентификатор инструмента
    /// </summary>
    [Column("figi")]
    public string Figi { get; set; }
    
    /// <summary>
    /// Тикер инструмента
    /// </summary>
    [Column("ticker")]
    public string Ticker { get; set; }
    
    /// <summary>
    /// Цена инструмента
    /// </summary>
    [Column("price")]
    public double Price { get; set; }
    
    /// <summary>
    /// Класс-код инструмента
    /// </summary>
    [Column("class_code")]
    public string ClassCode { get; set; }
    
    /// <summary>
    /// Валюта расчётов
    /// </summary>
    [Column("currency")]
    public string Currency { get; set; }
    
    /// <summary>
    /// Тип инструмента
    /// </summary>
    [Column("instrument_kind")]
    public string InstrumentKind { get; set; }
    
    /// <summary>
    /// Название инструмента
    /// </summary>
    [Column("name")]
    public string Name { get; set; }
    
    /// <summary>
    /// Tорговая площадка (секция биржи)
    /// </summary>
    [Column("exchange")]
    public string Exchange { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    [Column("uid")]
    public string Uid { get; set; }
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    [Column("in_watch_list")]
    public bool InWatchList { get; set; } = false; 
}