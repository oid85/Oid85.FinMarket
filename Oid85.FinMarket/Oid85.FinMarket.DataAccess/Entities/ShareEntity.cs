using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class ShareEntity : AuditableEntity
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
    /// Описание
    /// </summary>
    [Column("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Сектор
    /// </summary>
    [Column("sector")]
    public string Sector { get; set; } = string.Empty;
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    [Column("in_watch_list")]
    public bool InWatchList { get; set; } = false; 
}