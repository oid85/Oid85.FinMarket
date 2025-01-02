using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class AnalyseResultEntity : BaseEntity
{
    /// <summary>
    /// Id инструмента
    /// </summary>
    [Column("instrument_id")]
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Результат анализа
    /// </summary>
    [Column("result"), MaxLength(200)]
    public string Result { get; set; } = string.Empty;
    
    /// <summary>
    /// Тип анализа
    /// </summary>
    [Column("analyse_type"), MaxLength(20)]
    public string AnalyseType { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата
    /// </summary>
    [Column("date", TypeName = "date")]
    public DateOnly Date { get; set; }    
}