using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class StatisticalArbitrageStrategySignalEntity : AuditableEntity
{
    /// <summary>
    /// Тикер инструмента
    /// </summary>
    [Column("ticker_first"), MaxLength(20)]
    public string TickerFirst { get; set; } = string.Empty;
    
    /// <summary>
    /// Тикер инструмента
    /// </summary>
    [Column("ticker_second"), MaxLength(20)]
    public string TickerSecond { get; set; } = string.Empty;
    
    /// <summary>
    /// Количество сигналов
    /// </summary>
    [Column("count_signals")]
    public int CountSignals { get; set; }
    
    /// <summary>
    /// Количество стратегий
    /// </summary>
    [Column("count_strategies")]
    public int CountStrategies { get; set; }    
    
    /// <summary>
    /// Процент сигналов
    /// </summary>
    [Column("percent_signals")]
    public double PercentSignals { get; set; }    
    
    /// <summary>
    /// Размер позиции, руб
    /// </summary>
    [Column("position_cost")]
    public double PositionCost { get; set; }   
    
    /// <summary>
    /// Размер позиции, шт
    /// </summary>
    [Column("position_size_first")]
    public int PositionSizeFirst { get; set; } 
    
    /// <summary>
    /// Размер позиции, шт
    /// </summary>
    [Column("position_size_second")]
    public int PositionSizeSecond { get; set; } 
    
    /// <summary>
    /// Размер позиции в процентах от портфеля
    /// </summary>
    [Column("position_percent_portfolio")]
    public double PositionPercentPortfolio { get; set; }  
    
    /// <summary>
    /// Цена инструмента
    /// </summary>
    [Column("last_price_first")]
    public double LastPriceFirst { get; set; } 
    
    /// <summary>
    /// Цена инструмента
    /// </summary>
    [Column("last_price_second")]
    public double LastPriceSecond { get; set; } 
}