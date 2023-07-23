using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oid85.FinMarket.DAL.Entities;

[Table("market_events", Schema = "public")]
public class MarketEventEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Column("ticker")]
    public string Ticker { get; set; }
    
    [Column("market_event_type_id")]
    public long MarketEventTypeId { get; set; }
    
    [ForeignKey("MarketEventTypeId")]
    public MarketEventTypeEntity MarketEventTypeEntity { get; set; }
}