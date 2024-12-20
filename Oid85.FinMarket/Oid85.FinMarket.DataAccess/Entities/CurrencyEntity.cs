using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class CurrencyEntity : AuditableEntity
{
    /// <summary>
    /// Тикер
    /// </summary>
    [Column("ticker")]
    public string Ticker { get; set; } = string.Empty;

    /// <summary>
    /// Цена инструмента
    /// </summary>
    [Column("price")]
    public double Price { get; set; }
    
    /// <summary>
    /// Идентификатор ISIN
    /// </summary>
    [Column("isin")]
    public string Isin { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор FIGI
    /// </summary>
    [Column("figi")]
    public string Figi { get; set; } = string.Empty;
    
    /// <summary>
    /// Класс-код (секция торгов)
    /// </summary>
    [Column("class_code")]
    public string ClassCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Название инструмента
    /// </summary>
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Строковый ISO-код валюты
    /// </summary>
    [Column("iso_currency_name")]
    public string IsoCurrencyName { get; set; } = string.Empty;
    
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