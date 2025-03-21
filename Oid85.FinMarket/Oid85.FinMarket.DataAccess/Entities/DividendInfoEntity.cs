﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class DividendInfoEntity : AuditableEntity
{
    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    [Column("instrument_id")]
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Тикер
    /// </summary>
    [Column("ticker"), MaxLength(20)]
    public string Ticker { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата фиксации реестра
    /// </summary>
    [Column("record_date", TypeName = "date")]
    public DateOnly RecordDate { get; set; } = DateOnly.MinValue;

    /// <summary>
    /// Дата объявления дивидендов
    /// </summary>
    [Column("declared_date", TypeName = "date")]
    public DateOnly DeclaredDate { get; set; } = DateOnly.MinValue;

    /// <summary>
    /// Выплата, руб
    /// </summary>
    [Column("dividend")]
    public double Dividend { get; set; }

    /// <summary>
    /// Доходность, %
    /// </summary>
    [Column("dividend_prc")]
    public double DividendPrc { get; set; }
}