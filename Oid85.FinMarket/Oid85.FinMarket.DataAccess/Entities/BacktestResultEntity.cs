using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class BacktestResultEntity : AuditableEntity
{
    /// <summary>
    /// Тикер инструмента
    /// </summary>
    [Column("ticker"), MaxLength(20)]
    public string Ticker { get; set; } = string.Empty;
    
    /// <summary>
    /// Таймфрейм
    /// </summary>
    [Column("timeframe"), MaxLength(10)]
    public string Timeframe { get; set; } = string.Empty;
    
    /// <summary>
    /// Идентификатор стратегии
    /// </summary>
    [Column("strategy_id"), MaxLength(40)]
    public string StrategyId { get; set; } = string.Empty;
    
    /// <summary>
    /// Описание стратегии
    /// </summary>
    [Column("strategy_description"), MaxLength(1000)]
    public string StrategyDescription { get; set; } = string.Empty;
    
    /// <summary>
    /// Версия стратегии
    /// </summary>
    [Column("strategy_version")]
    public int StrategyVersion { get; set; }
    
    /// <summary>
    /// Параметры стратегии
    /// </summary>
    [Column("strategy_params"), MaxLength(1000)]
    public string StrategyParams { get; set; } = string.Empty;
    
    /// <summary>
    /// Параметры стратегии (хэш)
    /// </summary>
    [Column("strategy_params_hash"), MaxLength(100)]
    public string StrategyParamsHash { get; set; } = string.Empty;
    
    /// <summary>
    /// Количество сделок
    /// </summary>
    [Column("total")]
    public int Total { get; set; }
    
    /// <summary>
    /// Количество открытых сделок
    /// </summary>
    [Column("total_open")]
    public int TotalOpen { get; set; }
    
    /// <summary>
    /// Количество закрытых сделок
    /// </summary>
    [Column("total_closed")]
    public int TotalClosed { get; set; }    
    
    /// <summary>
    /// Макс. послед. убыточных сделок
    /// </summary>
    [Column("streak_won_longest")]
    public int StreakWonLongest { get; set; }
    
    /// <summary>
    /// Макс. послед. прибыльных сделок
    /// </summary>
    [Column("streak_lost_longest")]
    public int StreakLostLongest { get; set; }    
    
    /// <summary>
    /// Прибыль
    /// </summary>
    [Column("pnl_net_total")]
    public double PnlNetTotal { get; set; } 
    
    /// <summary>
    /// Прибыль средняя
    /// </summary>
    [Column("pnl_net_average")]
    public double PnlNetAverage { get; set; } 
    
    /// <summary>
    /// Максимальная просадка, %
    /// </summary>
    [Column("max_drawdown_percent")]
    public double MaxDrawdownPercent { get; set; }
    
    /// <summary>
    /// Profit Factor
    /// </summary>
    [Column("profit_factor")]
    public double ProfitFactor { get; set; }
    
    /// <summary>
    /// Recovery Factor
    /// </summary>
    [Column("recovery_factor")]
    public double RecoveryFactor { get; set; }
    
    /// <summary>
    /// Коэффициент Шарпа
    /// </summary>
    [Column("sharp_ratio")]
    public double SharpRatio { get; set; }
}