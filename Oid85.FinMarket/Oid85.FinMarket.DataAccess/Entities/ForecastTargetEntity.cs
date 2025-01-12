using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

/// <summary>
/// Прогноз
/// </summary>
public class ForecastTargetEntity : AuditableEntity
{
    /// <summary>
    /// Тикер
    /// </summary>
    [Column("ticker"), MaxLength(20)]
    public string Ticker { get; set; } = string.Empty;

    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    [Column("instrument_id")]
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Название компании, давшей прогноз
    /// </summary>
    [Column("company"), MaxLength(200)]
    public string Company { get; set; } = string.Empty;
    
    /// <summary>
    /// Прогноз строкой
    /// </summary>
    [Column("recommendation_string"), MaxLength(20)]
    public string RecommendationString { get; set; } = string.Empty;
    
    /// <summary>
    /// Прогноз числом
    /// </summary>
    [Column("recommendation_number")]
    public int RecommendationNumber { get; set; }
    
    /// <summary>
    /// Дата прогноза
    /// </summary>
    [Column("recommendation_date", TypeName = "date")]
    public DateOnly RecommendationDate { get; set; }
    
    /// <summary>
    /// Валюта
    /// </summary>
    [Column("currency"), MaxLength(20)]
    public string Currency { get; set; } = string.Empty;
    
    /// <summary>
    /// Текущая цена
    /// </summary>
    [Column("current_price")]
    public double CurrentPrice { get; set; }
    
    /// <summary>
    /// Прогнозируемая цена
    /// </summary>
    [Column("target_price")]
    public double TargetPrice { get; set; }
    
    /// <summary>
    /// Изменение цены
    /// </summary>
    [Column("price_change")]
    public double PriceChange { get; set; }
    
    /// <summary>
    /// Относительное изменение цены
    /// </summary>
    [Column("price_change_rel")]
    public double PriceChangeRel { get; set; }
    
    /// <summary>
    /// Наименование инструмента
    /// </summary>
    [Column("show_name"), MaxLength(200)]
    public string ShowName { get; set; } = string.Empty;
}