﻿namespace Oid85.FinMarket.Domain.Models;

/// <summary>
/// Дневная свеча
/// </summary>
public class DailyCandle
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Id инструмента
    /// </summary>
    public Guid InstrumentId { get; set; }

    /// <summary>
    /// Цена открытия
    /// </summary>
    public double Open { get; set; }

    /// <summary>
    /// Цена закрытия
    /// </summary>
    public double Close { get; set; }

    /// <summary>
    /// Макс. цена
    /// </summary>
    public double High { get; set; }

    /// <summary>
    /// Мин. цена
    /// </summary>
    public double Low { get; set; }

    /// <summary>
    /// Объем
    /// </summary>
    public long Volume { get; set; }

    /// <summary>
    /// Дата
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Свеча сформирована
    /// </summary>
    public bool IsComplete { get; set; }
}