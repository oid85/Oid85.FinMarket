using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class MultiplicatorEntity : AuditableEntity
{
    /// <summary>
    /// Тикер
    /// </summary>
    [Column("ticker"), MaxLength(20)]
    public string Ticker { get; set; } = string.Empty;
    
    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    [Column("instrument_id")]
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Рыночная капитализация
    /// </summary>
    [Column("market_capitalization")]
    public double MarketCapitalization { get; set; }
    
    /// <summary>
    /// Годовой минимум
    /// </summary>
    [Column("low_of_year")]
    public double LowOfYear { get; set; }
    
    /// <summary>
    /// Годовой максимум
    /// </summary>
    [Column("high_of_year")]
    public double HighOfYear { get; set; }
    
    /// <summary>
    /// Бета-коэффициент
    /// </summary>
    [Column("beta")]
    public double Beta { get; set; }
    
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