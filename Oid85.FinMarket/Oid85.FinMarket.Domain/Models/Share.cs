namespace Oid85.FinMarket.Domain.Models;

public class Share
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
    /// Цена инструмента
    /// </summary>
    public double Price { get; set; }
    
    /// <summary>
    /// Идентификатор ISIN
    /// </summary>
    public string Isin { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор FIGI
    /// </summary>
    public string Figi { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Сектор
    /// </summary>
    public string Sector { get; set; } = string.Empty;
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    public bool InWatchList { get; set; } = false; 
}