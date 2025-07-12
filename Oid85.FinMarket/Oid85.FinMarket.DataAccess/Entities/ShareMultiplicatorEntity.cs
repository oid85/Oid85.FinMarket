using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class ShareMultiplicatorEntity : AuditableEntity
{
    [Column("ticker"), MaxLength(20)]
    public string Ticker { get; set; } = string.Empty;
    
    [Column("name"), MaxLength(20)]
    public string Name { get; set; } = string.Empty;
    
    public double MarketCap { get; set; }
    
    public double Ev { get; set; }
    
    public double Revenue { get; set; }
}