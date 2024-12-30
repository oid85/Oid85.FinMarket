namespace Oid85.FinMarket.Domain.Models;

public class FinIndex
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// FIGI-идентификатор инструмента
    /// </summary>
    public string Figi { get; set; } = string.Empty;
    
    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Тикер инструмента
    /// </summary>
    public string Ticker { get; set; } = string.Empty;
    
    /// <summary>
    /// Цена инструмента
    /// </summary>
    public double Price { get; set; }
    
    /// <summary>
    /// Класс-код инструмента
    /// </summary>
    public string ClassCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Валюта расчётов
    /// </summary>
    public string Currency { get; set; } = string.Empty;
    
    /// <summary>
    /// Тип инструмента
    /// </summary>
    public string InstrumentKind { get; set; } = string.Empty;
    
    /// <summary>
    /// Название инструмента
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Tорговая площадка (секция биржи)
    /// </summary>
    public string Exchange { get; set; } = string.Empty;
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    public bool InWatchList { get; set; } = false; 
}