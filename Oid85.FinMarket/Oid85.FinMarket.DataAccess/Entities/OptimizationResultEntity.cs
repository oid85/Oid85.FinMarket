using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class OptimizationResultEntity : AuditableEntity
{
    /// <summary>
    /// Начало периода
    /// </summary>
    [Column("start_date", TypeName = "date")]
    public DateOnly StartDate { get; set; }
    
    /// <summary>
    /// Конец периода
    /// </summary>
    [Column("end_date", TypeName = "date")]
    public DateOnly EndDate { get; set; }
    
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
    [Column("strategy_id")]
    public Guid StrategyId { get; set; }
    
    /// <summary>
    /// Описание стратегии
    /// </summary>
    [Column("strategy_description"), MaxLength(1000)]
    public string StrategyDescription { get; set; } = string.Empty;
    
    /// <summary>
    /// Наименование стратегии
    /// </summary>
    [Column("strategy_name"), MaxLength(1000)]
    public string StrategyName { get; set; } = string.Empty;
    
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
    [Column("number_positions")]
    public int NumberPositions { get; set; }
    
    /// <summary>
    /// Текущая позиция
    /// </summary>
    [Column("current_position")]
    public int CurrentPosition { get; set; }    
    
    /// <summary>
    /// Текущая позиция (стоимость)
    /// </summary>
    [Column("current_position_cost")]
    public double CurrentPositionCost { get; set; } 
    
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
    /// Net Profit
    /// </summary>
    [Column("net_profit")]
    public double NetProfit { get; set; }
    
    /// <summary>
    /// Average Profit
    /// </summary>
    [Column("average_profit")]
    public double AverageProfit { get; set; }
    
    /// <summary>
    /// Average Profit Percent
    /// </summary>
    [Column("average_profit_percent")]
    public double AverageProfitPercent { get; set; }
    
    /// <summary>
    /// Drawdown
    /// </summary>
    [Column("drawdown")]
    public double Drawdown { get; set; }
    
    /// <summary>
    /// Max Drawdown
    /// </summary>
    [Column("max_drawdown")]
    public double MaxDrawdown { get; set; }
    
    /// <summary>
    /// MaxDrawdownPercent
    /// </summary>
    [Column("max_drawdown_percent")]
    public double MaxDrawdownPercent { get; set; }
    
    /// <summary>
    /// Winning Positions
    /// </summary>
    [Column("winning_positions")]
    public int WinningPositions { get; set; }
    
    /// <summary>
    /// Winning Trades Percent
    /// </summary>
    [Column("winning_trades_percent")]
    public double WinningTradesPercent { get; set; }
    
    /// <summary>
    /// Start Money
    /// </summary>
    [Column("start_money")]
    public double StartMoney { get; set; }
    
    /// <summary>
    /// End Money
    /// </summary>
    [Column("end_money")]
    public double EndMoney { get; set; }
    
    /// <summary>
    /// Доходность всего, %
    /// </summary>
    [Column("total_return")]
    public double TotalReturn { get; set; }
    
    /// <summary>
    /// Доходность годовая, %;
    /// </summary>
    [Column("annual_yield_return")]
    public double AnnualYieldReturn { get; set; }
}