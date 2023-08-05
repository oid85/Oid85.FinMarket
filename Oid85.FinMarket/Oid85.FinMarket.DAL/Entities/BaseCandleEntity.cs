using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oid85.FinMarket.DAL.Entities;

public class BaseCandleEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("datetime")]
    public DateTime DateTime { get; set; }
    
    [Column("open")]
    public double Open { get; set; }
    
    [Column("close")]
    public double Close { get; set; }
    
    [Column("high")]
    public double High { get; set; }
    
    [Column("low")]
    public double Low { get; set; }
    
    [Column("volume")]
    public long Volume { get; set; }
    
    [Column("ticker")]
    public string Ticker { get; set; }
}