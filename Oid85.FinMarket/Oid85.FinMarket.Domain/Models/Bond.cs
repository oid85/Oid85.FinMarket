﻿namespace Oid85.FinMarket.Domain.Models;

public class Bond
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

    /// <summary>
    /// Значение НКД (накопленного купонного дохода) на дату
    /// </summary>
    public double Nkd { get; set; }
    
    /// <summary>
    /// Дата погашения облигации по UTC
    /// </summary>
    public DateOnly MaturityDate { get; set; }
    
    /// <summary>
    /// Признак облигации с плавающим купоном
    /// </summary>
    public bool FloatingCouponFlag { get; set; }
    
    /// <summary>
    /// Уровень риска облигации
    /// </summary>
    public int RiskLevel { get; set; }

    /// <summary>
    /// Валюта расчетов
    /// </summary>
    public string Currency { get; set; } = string.Empty;
}