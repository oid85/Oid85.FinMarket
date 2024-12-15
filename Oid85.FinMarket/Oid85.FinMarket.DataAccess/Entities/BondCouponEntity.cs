using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class BondCouponEntity : AuditableEntity
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
    /// Дата выплаты купона
    /// </summary>
    [Column("coupon_date", TypeName = "date")]
    public DateOnly CouponDate { get; set; } = DateOnly.MinValue;
    
    /// <summary>
    /// Номер купона
    /// </summary>
    [Column("coupon_number")]
    public long CouponNumber { get; set; }
    
    /// <summary>
    /// Купонный период в днях
    /// </summary>
    [Column("coupon_period")]
    public int CouponPeriod { get; set; }

    /// <summary>
    /// Начало купонного периода
    /// </summary>
    [Column("coupon_start_date", TypeName = "date")]
    public DateOnly CouponStartDate { get; set; } = DateOnly.MinValue;
    
    /// <summary>
    /// Окончание купонного периода
    /// </summary>
    [Column("coupon_end_date", TypeName = "date")]
    public DateOnly CouponEndDate { get; set; } = DateOnly.MinValue;
    
    /// <summary>
    /// Выплата на одну облигацию
    /// </summary>
    [Column("pay_one_bond")]
    public double PayOneBond { get; set; }
}