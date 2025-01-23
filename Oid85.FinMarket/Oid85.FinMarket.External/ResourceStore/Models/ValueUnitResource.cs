namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Значение - единица измерения
/// </summary>
public class ValueUnitResource
{
    /// <summary>
    /// Значение
    /// </summary>
    public double Value { get; set; }
    
    /// <summary>
    /// Единица изиерения
    /// </summary>
    public string Unit { get; set; } = string.Empty;
}

