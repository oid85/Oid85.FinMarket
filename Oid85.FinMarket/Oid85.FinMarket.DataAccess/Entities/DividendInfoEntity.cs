using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class DividendInfoEntity : AuditableEntity
{
    /// <summary>
    /// Дата фиксации реестра
    /// </summary>
    [Column("record_date")]
    public DateTime RecordDate { get; set; } = DateTime.MinValue.ToUniversalTime();

    /// <summary>
    /// Дата объявления дивидендов
    /// </summary>
    [Column("declared_date")]
    public DateTime DeclaredDate { get; set; } = DateTime.MinValue.ToUniversalTime();

    /// <summary>
    /// Выплата, руб
    /// </summary>
    [Column("dividend")]
    public double Dividend { get; set; } = 0.0;

    /// <summary>
    /// Доходность, %
    /// </summary>
    [Column("dividend_prc")]
    public double DividendPrc { get; set; } = 0.0;
    
    [Column("share_id")]
    public Guid ShareId { get; set; }
    
    public ShareEntity Share { get; set; } = new();
}