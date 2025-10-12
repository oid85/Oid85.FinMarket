namespace Oid85.FinMarket.Domain.Models.PairArbitrage;

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