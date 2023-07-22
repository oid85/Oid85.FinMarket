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
    public decimal Open { get; set; }
    
    [Column("close")]
    public decimal Close { get; set; }
    
    [Column("high")]
    public decimal High { get; set; }
    
    [Column("low")]
    public decimal Low { get; set; }
    
    [Column("volume")]
    public decimal Volume { get; set; }
    
    [Column("ticker")]
    public string Ticker { get; set; }
}