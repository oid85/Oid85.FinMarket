using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class RegressionTailEntity : AuditableEntity
{
    /// <summary>
    /// Тикер инструмента 1
    /// </summary>
    [Column("ticker1"), MaxLength(20)]
    public string Ticker1 { get; set; } = string.Empty; 
    
    /// <summary>
    /// Тикер инструмента 2
    /// </summary>
    [Column("ticker2"), MaxLength(20)]
    public string Ticker2 { get; set; } = string.Empty; 
    
    /// <summary>
    /// Хвосты
    /// </summary>
    [Column("tails")]
    public string Tails { get; set; } = string.Empty; 
    
    /// <summary>
    /// Признак стационарности
    /// </summary>
    [Column("is_stationary")]
    public bool IsStationary { get; set; }
    
    /// <summary>
    /// Наклон
    /// </summary>
    [Column("slope")]
    public double Slope { get; set; } 
    
    /// <summary>
    /// Пересечение
    /// </summary>
    [Column("intercept")]
    public double Intercept { get; set; }
}