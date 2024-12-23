using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class AnalyseResultEntity : BaseEntity
{
    /// <summary>
    /// Тикер
    /// </summary>
    [Column("ticker"), MaxLength(10)]
    public string Ticker { get; set; } = string.Empty;

    /// <summary>
    /// Таймфрейм
    /// </summary>
    [Column("timeframe"), MaxLength(20)]
    public string Timeframe { get; set; } = string.Empty;    
    
    /// <summary>
    /// Результат анализа
    /// </summary>
    [Column("result"), MaxLength(20)]
    public string Result { get; set; } = string.Empty;
    
    /// <summary>
    /// Тип анализа
    /// </summary>
    [Column("analyse_type"), MaxLength(20)]
    public string AnalyseType { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата
    /// </summary>
    [Column("date", TypeName = "timestamp with time zone")]
    public DateTime Date { get; set; }    
}