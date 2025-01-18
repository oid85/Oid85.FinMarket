namespace Oid85.FinMarket.Domain.Models;

public class MarketEvent
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
    /// Дата
    /// </summary>
    public TimeOnly Time { get; set; } 
    
    /// <summary>
    /// Тикер
    /// </summary>
    public string Ticker { get; set; } = string.Empty;
    
    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Тип события
    /// </summary>
    public string MarketEventType { get; set; } = string.Empty;
    
    /// <summary>
    /// Активно/неактивно
    /// </summary>
    public bool IsActive { get; set; } = false;
}