using System.ComponentModel.DataAnnotations.Schema;

namespace Oid85.FinMarket.DataAccess.Entities;

public class FiveMinuteCandleEntity : CandleEntity
{
    /// <summary>
    /// Время
    /// </summary>
    [Column("time", TypeName = "time")]
    public TimeOnly Time { get; set; }
}