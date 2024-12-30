using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class BondEntity : AuditableEntity
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
    /// Уникальный идентификатор инструмента
    /// </summary>
    [Column("instrument_id")]
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Наименование
    /// </summary>
    [Column("name"), MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Сектор
    /// </summary>
    [Column("sector"), MaxLength(20)]
    public string Sector { get; set; } = string.Empty;
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    [Column("in_watch_list")]
    public bool InWatchList { get; set; }
    
    /// <summary>
    /// Значение НКД (накопленного купонного дохода) на дату
    /// </summary>
    [Column("nkd")]
    public double Nkd { get; set; }

    /// <summary>
    /// Дата погашения облигации по UTC
    /// </summary>
    [Column("maturity_date", TypeName = "date")]
    public DateOnly MaturityDate { get; set; } = DateOnly.MinValue;
    
    /// <summary>
    /// Признак облигации с плавающим купоном
    /// </summary>
    [Column("floating_coupon_flag")]
    public bool FloatingCouponFlag { get; set; }
}