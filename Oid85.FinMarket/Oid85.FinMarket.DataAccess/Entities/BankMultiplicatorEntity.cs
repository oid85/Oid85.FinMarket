using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class BankMultiplicatorEntity : AuditableEntity
{
    [Column("ticker"), MaxLength(20)]
    public string Ticker { get; set; } = string.Empty;
    
    [Column("name"), MaxLength(500)]
    public string Name { get; set; } = string.Empty;
    
    [Column("market_cap")]
    public double MarketCap { get; set; }
    
    [Column("net_income")]
    public double NetIncome { get; set; }
    
    [Column("dd_ao")]
    public double DdAo { get; set; }
    
    [Column("dd_ap")]
    public double DdAp { get; set; }
    
    [Column("dd_net_income")]
    public double DdNetIncome { get; set; }
    
    [Column("pe")]
    public double Pe { get; set; }
    
    [Column("pb")]
    public double Pb { get; set; }

    [Column("net_operating_income")]
    public double NetOperatingIncome { get; set; }
    
    [Column("net_interest_margin")]
    public double NetInterestMargin { get; set; }
    
    [Column("roe")]
    public double Roe { get; set; }
    
    [Column("roa")]
    public double Roa { get; set; }
}