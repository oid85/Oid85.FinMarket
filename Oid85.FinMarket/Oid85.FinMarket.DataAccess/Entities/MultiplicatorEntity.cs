using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class MultiplicatorEntity : AuditableEntity
{
    /// <summary>
    /// Тикер, ао
    /// </summary>
    [Column("ticker_ao"), MaxLength(20)]
    public string TickerAo { get; set; } = string.Empty;
    
    /// <summary>
    /// Тикер, ап
    /// </summary>
    [Column("ticker_ap"), MaxLength(20)]
    public string TickerAp { get; set; } = string.Empty;
    
    /// <summary>
    /// Количество обыкновенных акций
    /// </summary>
    [Column("total_shares_ao")]
    public double TotalSharesAo { get; set; }
    
    /// <summary>
    /// Количество привелегированных акций
    /// </summary>
    [Column("total_shares_ap")]
    public double TotalSharesAp { get; set; }
    
    /// <summary>
    /// Бета-коэффициент
    /// </summary>
    [Column("beta")]
    public double Beta { get; set; }
    
    /// <summary>
    /// Выручка
    /// </summary>
    [Column("revenue")]
    public double Revenue { get; set; }
    
    /// <summary>
    /// Операционная прибыль
    /// </summary>
    [Column("operating_income")]
    public double OperatingIncome { get; set; }
    
    /// <summary>
    /// P/E
    /// </summary>
    [Column("pe")]
    public double Pe { get; set; }
    
    /// <summary>
    /// P/B
    /// </summary>
    [Column("pb")]
    public double Pb { get; set; }
    
    /// <summary>
    /// P/BV
    /// </summary>
    [Column("pbv")]
    public double Pbv { get; set; }
    
    /// <summary>
    /// EV
    /// </summary>
    [Column("ev")]
    public double Ev { get; set; }
    
    /// <summary>
    /// ROE
    /// </summary>
    [Column("roe")]
    public double Roe { get; set; }
    
    /// <summary>
    /// ROA
    /// </summary>
    [Column("roa")]
    public double Roa { get; set; }
    
    /// <summary>
    /// Чистая процентная маржа
    /// </summary>
    [Column("net_interest_margin")]
    public double NetInterestMargin { get; set; }
    
    /// <summary>
    /// Долг
    /// </summary>
    [Column("total_debt")]
    public double TotalDebt { get; set; }
    
    /// <summary>
    /// Чистый долг
    /// </summary>
    [Column("net_debt")]
    public double NetDebt { get; set; }
    
    /// <summary>
    /// Рыночная капитализация
    /// </summary>
    [Column("market_capitalization")]
    public double MarketCapitalization { get; set; }
    
    /// <summary>
    /// Чистая прибыль
    /// </summary>
    [Column("net_income")]
    public double NetIncome { get; set; }
    
    /// <summary>
    /// EBITDA
    /// </summary>
    [Column("ebitda")]
    public double Ebitda { get; set; }
    
    /// <summary>
    /// Прибыль на акцию
    /// </summary>
    [Column("eps")]
    public double Eps { get; set; }
    
    /// <summary>
    /// Свободный денежный поток
    /// </summary>
    [Column("free_cash_flow")]
    public double FreeCashFlow { get; set; }
    
    /// <summary>
    /// EV / EBITDA
    /// </summary>
    [Column("ev_to_ebitda")]
    public double EvToEbitda { get; set; }
    
    /// <summary>
    /// Долг / EBITDA
    /// </summary>
    [Column("total_debt_to_ebitda")]
    public double TotalDebtToEbitda { get; set; }
    
    /// <summary>
    /// Чистый долг / EBITDA
    /// </summary>
    [Column("net_debt_to_ebitda")]
    public double NetDebtToEbitda { get; set; }
}