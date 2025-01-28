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
    public double LastPrice { get; set; }
    
    /// <summary>
    /// Нижний целевой уровень
    /// </summary>
    public double HighTargetPrice { get; set; }
    
    /// <summary>
    /// Верхний целевой уровень
    /// </summary>
    public double LowTargetPrice { get; set; }
    
    /// <summary>
    /// Идентификатор ISIN
    /// </summary>
    public string Isin { get; set; } = string.Empty;

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
    /// Сектор
    /// </summary>
    public string Sector { get; set; } = string.Empty;
}