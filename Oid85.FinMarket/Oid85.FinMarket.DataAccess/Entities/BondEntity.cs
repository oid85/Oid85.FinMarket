﻿using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class BondEntity : AuditableEntity
{
    /// <summary>
    /// Тикер
    /// </summary>
    [Column("ticker")]
    public string Ticker { get; set; } = string.Empty;

    /// <summary>
    /// Цена инструмента
    /// </summary>
    [Column("price")]
    public double Price { get; set; }
    
    /// <summary>
    /// Идентификатор ISIN
    /// </summary>
    [Column("isin")]
    public string Isin { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор FIGI
    /// </summary>
    [Column("figi")]
    public string Figi { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    [Column("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Сектор
    /// </summary>
    [Column("sector")]
    public string Sector { get; set; } = string.Empty;
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    [Column("in_watch_list")]
    public bool InWatchList { get; set; }
    
    /// <summary>
    /// Значение НКД (накопленного купонного дохода) на дату
    /// </summary>
    [Column("nkd")]
    public double NKD { get; set; }

    /// <summary>
    /// Дата погашения облигации по UTC
    /// </summary>
    [Column("maturity_date", TypeName = "date")]
    public DateOnly MaturityDate { get; set; } = DateOnly.MinValue;
    
    /// <summary>
    /// Признак облигации с плавающим купоном
    /// </summary>
    [Column("floating_coupon_flag")]
    public bool FloatingCouponFlag { get; set; }
}