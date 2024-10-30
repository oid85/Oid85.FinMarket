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
    /// Флаг активности
    /// </summary>
    [Column("is_active")]
    public bool IsActive { get; set; } = true;   
    
    /// <summary>
    /// Находится в составе индекса Московской биржи
    /// </summary>
    [Column("in_irus_index")]
    public bool InIrusIndex { get; set; } = false;
    
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