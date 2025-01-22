namespace Oid85.FinMarket.Application.Models.Resources;

/// <summary>
/// Цвет диапазона
/// </summary>
public class RangeColorResource
{
    /// <summary>
    /// Верхний уровень
    /// </summary>
    public double HighLevel { get; set; }
    
    /// <summary>
    /// Нижний уровень
    /// </summary>
    public double LowLevel { get; set; }
    
    /// <summary>
    /// Код цвета (RGB)
    /// </summary>
    public string ColorCode { get; set; } = string.Empty;
}
