using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class ShareMultiplicatorEntity : AuditableEntity
{
    [Column("ticker"), MaxLength(20)]
    public string Ticker { get; set; } = string.Empty;
    
    [Column("name"), MaxLength(500)]
    public string Name { get; set; } = string.Empty;
    
    [Column("market_cap")]
    public double MarketCap { get; set; }
    
    [Column("ev")]
    public double Ev { get; set; }
    
    [Column("revenue")]
    public double Revenue { get; set; } 
    
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
    
    [Column("ps")]
    public double Ps { get; set; }
    
    [Column("pb")]
    public double Pb { get; set; }
    
    [Column("ev_ebitda")]
    public double EvEbitda { get; set; }
    
    [Column("ebitda_margin")]
    public double EbitdaMargin { get; set; }
    
    [Column("net_debt_ebitda")]
    public double NetDebtEbitda { get; set; }
}