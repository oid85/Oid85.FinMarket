namespace Oid85.FinMarket.Domain.Models;

public class Ticker
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Наименование тикера
    /// </summary>
    public string Name { get; set; } = string.Empty;
}