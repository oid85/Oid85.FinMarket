using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oid85.FinMarket.DAL.Entities;

[Table("finance_instruments", Schema = "public")]
public class FinanceInstrumentEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Column("ticker")]
    public string Ticker { get; set; }
}