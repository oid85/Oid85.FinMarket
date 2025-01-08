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
    [Column("result_string"), MaxLength(200)]
    public string ResultString { get; set; } = string.Empty;
    
    /// <summary>
    /// Результат анализа числом
    /// </summary>
    [Column("result_number")]
    public double ResultNumber { get; set; }
    
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