namespace Oid85.FinMarket.Application.Models.Resources;

/// <summary>
/// Код фьючерса (месячный)
/// </summary>
public class FutureCodeResource
{
    /// <summary>
    /// Суффикс
    /// </summary>
    public string Suffix { get; set; } = string.Empty;
    
    /// <summary>
    /// Месяц
    /// </summary>
    public string Month { get; set; } = string.Empty;
}

