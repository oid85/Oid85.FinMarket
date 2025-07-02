using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class StrategySignalEntity : AuditableEntity
{
    /// <summary>
    /// Тикер инструмента
    /// </summary>
    [Column("ticker"), MaxLength(20)]
    public string Ticker { get; set; } = string.Empty; 
    
    /// <summary>
    /// Количество сигналов
    /// </summary>
    [Column("count_signals")]
    public int CountSignals { get; set; }
    
    /// <summary>
    /// Размер позиции
    /// </summary>
    [Column("position_cost")]
    public double PositionCost { get; set; }    
}