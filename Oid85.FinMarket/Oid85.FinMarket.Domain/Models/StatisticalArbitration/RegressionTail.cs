namespace Oid85.FinMarket.Domain.Models.StatisticalArbitration;

public class RegressionTail
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Тикер инструмента 1
    /// </summary>
    public string Ticker1 { get; set; } = string.Empty; 
    
    /// <summary>
    /// Тикер инструмента 2
    /// </summary>
    public string Ticker2 { get; set; } = string.Empty;

    /// <summary>
    /// Хвост
    /// </summary>
    public List<double> Tails { get; set; } = new();
    
    /// <summary>
    /// Даты
    /// </summary>
    public List<DateOnly> Dates { get; set; } = new();
    
    /// <summary>
    /// Признак стационарности
    /// </summary>
    public bool IsStationary { get; set; } = true;
}