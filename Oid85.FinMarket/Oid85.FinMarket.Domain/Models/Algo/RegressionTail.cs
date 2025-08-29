namespace Oid85.FinMarket.Domain.Models.Algo;

public class RegressionTail
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
    /// Хвосты
    /// </summary>
    public List<RegressionTailItem> Tails { get; set; } = new();
    
    /// <summary>
    /// Признак стационарности
    /// </summary>
    public bool IsStationary { get; set; } = true;
    
    /// <summary>
    /// Наклон
    /// </summary>
    public double Slope { get; set; } 
    
    /// <summary>
    /// Пересечение
    /// </summary>
    public double Intercept { get; set; }
}