namespace Oid85.FinMarket.Domain.Models.Algo;

public class Correlation
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Тикер инструмента 1
    /// </summary>
    public string TickerFirst { get; set; } = string.Empty; 
    
    /// <summary>
    /// Тикер инструмента 2
    /// </summary>
    public string TickerSecond { get; set; } = string.Empty; 
    
    /// <summary>
    /// Значение корреляции
    /// </summary>
    public double Value { get; set; }
}