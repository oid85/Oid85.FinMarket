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
    /// Дата выплаты купона
    /// </summary>
    [Column("coupon_date", TypeName = "timestamp with time zone")]
    public DateTime CouponDate { get; set; }
    
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
    [Column("coupon_start_date", TypeName = "timestamp with time zone")]
    public DateTime CouponStartDate { get; set; }
    
    /// <summary>
    /// Окончание купонного периода
    /// </summary>
    [Column("coupon_end_date", TypeName = "timestamp with time zone")]
    public DateTime CouponEndDate { get; set; }
    
    /// <summary>
    /// Выплата на одну облигацию
    /// </summary>
    [Column("pay_one_bond")]
    public double PayOneBond { get; set; }
}