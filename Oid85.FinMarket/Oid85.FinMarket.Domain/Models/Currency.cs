﻿namespace Oid85.FinMarket.Domain.Models;

public class Currency
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
    /// Класс-код (секция торгов)
    /// </summary>
    public string ClassCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Название инструмента
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Строковый ISO-код валюты
    /// </summary>
    public string IsoCurrencyName { get; set; } = string.Empty;
    
    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    public Guid InstrumentId { get; set; }
}