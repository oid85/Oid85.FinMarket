using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class InstrumentEntity : BaseEntity
{
    /// <summary>
    /// Id инструмента из Tinkoff API
    /// </summary>
    [Column("instrument_id")]
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Наименование тикера
    /// </summary>
    [Column("ticker"), MaxLength(20)]
    public string Ticker { get; set; } = string.Empty;
    
    /// <summary>
    /// Имя инструмента
    /// </summary>
    [Column("name"), MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Сектор
    /// </summary>
    [Column("sector"), MaxLength(200)]
    public string Sector { get; set; } = string.Empty;
    
    /// <summary>
    /// Тип инструмента
    /// </summary>
    [Column("type"), MaxLength(20)]
    public string Type { get; set; } = string.Empty;
}