using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class AssetReportEventEntity : BaseEntity
{
    /// <summary>
    /// Id инструмента из Tinkoff API
    /// </summary>
    [Column("instrument_id")]
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Дата публикации отчета
    /// </summary>
    [Column("report_date")]
    public DateOnly ReportDate { get; set; }
    
    /// <summary>
    /// Год периода отчета
    /// </summary>
    [Column("period_year")]
    public int PeriodYear { get; set; }
    
    /// <summary>
    /// Номер периода
    /// </summary>
    [Column("period_num")]
    public int PeriodNum { get; set; }
    
    /// <summary>
    /// Тип отчета
    /// </summary>
    [Column("type"), MaxLength(200)]
    public string Type { get; set; } = string.Empty;
}