using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class AnalyseResultEntity : BaseEntity
{
    /// <summary>
    /// Тикер
    /// </summary>
    [Column("ticker")]
    public string Ticker { get; set; } = string.Empty;

    /// <summary>
    /// Таймфрейм
    /// </summary>
    [Column("timeframe")]
    public string Timeframe { get; set; } = string.Empty;    
    
    /// <summary>
    /// Результат анализа
    /// </summary>
    [Column("analyse_result_type_id")]
    public string Result { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата
    /// </summary>
    [Column("date", TypeName = "timestamp with time zone")]
    public DateTime Date { get; set; }    
}