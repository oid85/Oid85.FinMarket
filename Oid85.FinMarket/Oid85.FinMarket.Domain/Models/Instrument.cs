namespace Oid85.FinMarket.Domain.Models;

public class Instrument
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
    /// Наименование тикера
    /// </summary>
    public string Ticker { get; set; } = string.Empty;
    
    /// <summary>
    /// Имя инструмента
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Тип инструмента
    /// </summary>
    public string Type { get; set; } = string.Empty;
}