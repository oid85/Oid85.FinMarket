namespace Oid85.FinMarket.Domain.Models;

/// <summary>
/// Прогноз
/// </summary>
public class ForecastTarget
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
    /// Уникальный идентификатор инструмента
    /// </summary>
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Название компании, давшей прогноз
    /// </summary>
    public string Company { get; set; } = string.Empty;
    
    /// <summary>
    /// Прогноз строкой
    /// </summary>
    public string RecommendationString { get; set; } = string.Empty;
    
    /// <summary>
    /// Прогноз числом
    /// </summary>
    public int RecommendationNumber { get; set; }
    
    /// <summary>
    /// Дата прогноза
    /// </summary>
    public DateOnly RecommendationDate { get; set; }
    
    /// <summary>
    /// Валюта
    /// </summary>
    public string Currency { get; set; } = string.Empty;
    
    /// <summary>
    /// Текущая цена
    /// </summary>
    public double CurrentPrice { get; set; }
    
    /// <summary>
    /// Прогнозируемая цена
    /// </summary>
    public double TargetPrice { get; set; }
    
    /// <summary>
    /// Изменение цены
    /// </summary>
    public double PriceChange { get; set; }
    
    /// <summary>
    /// Относительное изменение цены
    /// </summary>
    public double PriceChangeRel { get; set; }
    
    /// <summary>
    /// Наименование инструмента
    /// </summary>
    public string ShowName { get; set; } = string.Empty;
}