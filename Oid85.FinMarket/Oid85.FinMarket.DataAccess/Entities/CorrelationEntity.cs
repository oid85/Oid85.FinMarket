using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class CorrelationEntity : AuditableEntity
{
    /// <summary>
    /// Тикер инструмента 1
    /// </summary>
    [Column("ticker_first"), MaxLength(20)]
    public string TickerFirst { get; set; } = string.Empty; 
    
    /// <summary>
    /// Тикер инструмента 2
    /// </summary>
    [Column("ticker_second"), MaxLength(20)]
    public string TickerSecond { get; set; } = string.Empty; 
    
    /// <summary>
    /// Значение корреляции
    /// </summary>
    [Column("value")]
    public double Value { get; set; }
}