﻿namespace Oid85.FinMarket.Application.Models.Resources;

/// <summary>
/// Ценовой уровень
/// </summary>
public class PriceLevelResource
{
    /// <summary>
    /// Уровень включен
    /// </summary>
    public bool Enable { get; set; }
    
    /// <summary>
    /// Наименвание
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Значение
    /// </summary>
    public double Value { get; set; }
}

