using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oid85.FinMarket.DAL.Entities;

[Table("assets", Schema = "public")]
public class AssetEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Column("ticker")]
    public string Ticker { get; set; }
    
    [Column("name")]
    public string Name { get; set; }    
    
    [Column("figi")]
    public string Figi { get; set; }    
    
    [Column("sector")]
    public string Sector { get; set; }
}