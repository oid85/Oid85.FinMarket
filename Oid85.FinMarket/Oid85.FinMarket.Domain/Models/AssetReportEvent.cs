namespace Oid85.FinMarket.Domain.Models;

public class AssetReportEvent
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Id инструмента из Tinkoff API
    /// </summary>
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Дата публикации отчета
    /// </summary>
    public DateOnly ReportDate { get; set; }
    
    /// <summary>
    /// Год периода отчета
    /// </summary>
    public int PeriodYear { get; set; }
    
    /// <summary>
    /// Номер периода
    /// </summary>
    public int PeriodNum { get; set; }
    
    /// <summary>
    /// Тип отчета
    /// </summary>
    public string Type { get; set; } = string.Empty;
}