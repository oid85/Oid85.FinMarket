using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class FutureEntity : AuditableEntity
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
    /// Дата истечения срока
    /// </summary>
    [Column("expiration_date", TypeName = "date")]
    public DateOnly ExpirationDate = DateOnly.MinValue;
    
    /// <summary>
    /// Флаг активности
    /// </summary>
    [Column("is_active")]
    public bool IsActive { get; set; } = true;   
    
    /// <summary>
    /// Находится в портфеле
    /// </summary>
    [Column("in_portfolio")]
    public bool InPortfolio { get; set; } = false; 
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    [Column("in_watch_list")]
    public bool InWatchList { get; set; } = false; 
}