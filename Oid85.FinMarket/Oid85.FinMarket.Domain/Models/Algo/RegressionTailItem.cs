namespace Oid85.FinMarket.Domain.Models.Algo;

public class RegressionTailItem
{
    /// <summary>
    /// Дата
    /// </summary>
    public DateOnly Date { get; set; }
    
    /// <summary>
    /// Значение
    /// </summary>
    public double Value { get; set; } 
}