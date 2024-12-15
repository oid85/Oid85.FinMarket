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
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата истечения срока
    /// </summary>
    public DateOnly ExpirationDate = DateOnly.MinValue;
    
    /// <summary>
    /// Флаг активности
    /// </summary>
    public bool IsActive { get; set; } = true;   
    
    /// <summary>
    /// Находится в портфеле
    /// </summary>
    public bool InPortfolio { get; set; } = false; 
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    public bool InWatchList { get; set; } = false;
}