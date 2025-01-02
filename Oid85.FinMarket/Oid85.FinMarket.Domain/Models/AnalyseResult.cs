namespace Oid85.FinMarket.Domain.Models;

/// <summary>
/// Объект результата анализа
/// </summary>
public class AnalyseResult
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Дата
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Тип анализа
    /// </summary>
    public string AnalyseType { get; set; } = string.Empty;        
        
    /// <summary>
    /// Результат анализа
    /// </summary>
    public string Result { get; set; } = string.Empty;
}