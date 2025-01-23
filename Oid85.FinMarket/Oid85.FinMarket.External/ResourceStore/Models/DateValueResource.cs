namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Дата - значение
/// </summary>
public class DateValueResource
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

