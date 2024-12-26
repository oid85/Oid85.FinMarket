namespace Oid85.FinMarket.Domain.Models;

public class Future
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
    /// Идентификатор FIGI
    /// </summary>
    public string Figi { get; set; } = string.Empty;

    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата истечения срока
    /// </summary>
    public DateOnly ExpirationDate = DateOnly.MinValue;
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    public bool InWatchList { get; set; } = false;
}