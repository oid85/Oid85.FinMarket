using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class BondEntity : AuditableEntity
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
    /// Находится в портфеле
    /// </summary>
    [Column("in_portfolio")]
    public bool InPortfolio { get; set; }
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    [Column("in_watch_list")]
    public bool InWatchList { get; set; }
    
    /// <summary>
    /// Значение НКД (накопленного купонного дохода) на дату
    /// </summary>
    [Column("nkd")]
    public double NKD { get; set; }
    
    /// <summary>
    /// Дата погашения облигации по UTC
    /// </summary>
    [Column("maturity_date", TypeName = "timestamp with time zone")]
    public DateTime MaturityDate { get; set; }
    
    /// <summary>
    /// Признак облигации с плавающим купоном
    /// </summary>
    [Column("floating_coupon_flag")]
    public bool FloatingCouponFlag { get; set; }
}