namespace Oid85.FinMarket.Domain.Models;

public class Timeframe
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;
}