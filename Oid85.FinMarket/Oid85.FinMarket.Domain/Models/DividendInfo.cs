namespace Oid85.FinMarket.Domain.Models;

/// <summary>
/// Информация по дивидендам
/// </summary>
public class DividendInfo
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
        
    /// <summary>
    /// Тикер
    /// </summary>
    public string Ticker { get; set; } = string.Empty;

    /// <summary>
    /// Дата фиксации реестра
    /// </summary>
    public DateOnly RecordDate { get; set; }

    /// <summary>
    /// Дата объявления дивидендов
    /// </summary>
    public DateOnly DeclaredDate { get; set; }

    /// <summary>
    /// Выплата, руб
    /// </summary>
    public double Dividend { get; set; }

    /// <summary>
    /// Доходность, %
    /// </summary>
    public double DividendPrc { get; set; }
        
    /// <summary>
    /// Акция
    /// </summary>
    public Share Share { get; set; } = new();
}