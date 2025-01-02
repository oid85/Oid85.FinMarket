using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class CurrencyEntity : AuditableEntity
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
    /// Класс-код (секция торгов)
    /// </summary>
    [Column("class_code"), MaxLength(20)]
    public string ClassCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Название инструмента
    /// </summary>
    [Column("name"), MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Строковый ISO-код валюты
    /// </summary>
    [Column("iso_currency_name"), MaxLength(10)]
    public string IsoCurrencyName { get; set; } = string.Empty;
    
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