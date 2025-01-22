namespace Oid85.FinMarket.Application.Models.Resources;

/// <summary>
/// Дата-значение
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

