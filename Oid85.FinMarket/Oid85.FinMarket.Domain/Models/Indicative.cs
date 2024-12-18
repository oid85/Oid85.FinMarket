namespace Oid85.FinMarket.Domain.Models;

public class Indicative
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// FIGI-идентификатор инструмента
    /// </summary>
    public string Figi { get; set; }
    
    /// <summary>
    /// Тикер инструмента
    /// </summary>
    public string Ticker { get; set; }
    
    /// <summary>
    /// Цена инструмента
    /// </summary>
    public double Price { get; set; }
    
    /// <summary>
    /// Класс-код инструмента
    /// </summary>
    public string ClassCode { get; set; }
    
    /// <summary>
    /// Валюта расчётов
    /// </summary>
    public string Currency { get; set; }
    
    /// <summary>
    /// Тип инструмента
    /// </summary>
    public string InstrumentKind { get; set; }
    
    /// <summary>
    /// Название инструмента
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Tорговая площадка (секция биржи)
    /// </summary>
    public string Exchange { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    public string Uid { get; set; }
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    public bool InWatchList { get; set; } = false; 
}