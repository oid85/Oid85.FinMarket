using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class FutureEntity : AuditableEntity
{
    /// <summary>
    /// Тикер
    /// </summary>
    [Column("ticker"), MaxLength(20)]
    public string Ticker { get; set; } = string.Empty;

    /// <summary>
    /// Цена инструмента
    /// </summary>
    [Column("last_price")]
    public double LastPrice { get; set; }

    /// <summary>
    /// Идентификатор FIGI
    /// </summary>
    [Column("figi"), MaxLength(20)]
    public string Figi { get; set; } = string.Empty;

    /// <summary>
    /// Уникальный идентификатор инструмента
    /// </summary>
    [Column("instrument_id")]
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    [Column("name"), MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата истечения срока
    /// </summary>
    [Column("expiration_date", TypeName = "date")]
    public DateOnly ExpirationDate { get; set; } = DateOnly.MinValue;
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    [Column("in_watch_list")]
    public bool InWatchList { get; set; } = false;

    /// <summary>
    /// Лотность инструмента
    /// </summary>
    [Column("lot")]
    public int Lot { get; set; } = 1;

    /// <summary>
    /// Дата начала обращения контракта по UTC
    /// </summary>
    [Column("first_trade_date", TypeName = "date")]
    public DateOnly FirstTradeDate { get; set; }

    /// <summary>
    /// Дата по UTC, до которой возможно проведение операций с фьючерсом
    /// </summary>
    [Column("last_trade-date", TypeName = "date")]
    public DateOnly LastTradeDate { get; set; }

    /// <summary>
    /// Тип фьючерса
    /// </summary>
    [Column("future_type"), MaxLength(100)]
    public string FutureType { get; set; } = string.Empty;

    /// <summary>
    /// Тип актива
    /// </summary>
    [Column("asset_type"), MaxLength(20)]
    public string AssetType { get; set; } = string.Empty;

    /// <summary>
    /// Основной актив
    /// </summary>
    [Column("basic_asset"), MaxLength(200)]
    public string BasicAsset { get; set; } = string.Empty;

    /// <summary>
    /// Размер основного актива
    /// </summary>
    [Column("basic_asset_size")]
    public double BasicAssetSize { get; set; }

    /// <summary>
    /// Гарантийное обеспечение при покупке
    /// </summary>
    [Column("initial_margin_on_buy")]
    public double InitialMarginOnBuy { get; set; }

    /// <summary>
    /// Гарантийное обеспечение при продаже
    /// </summary>
    [Column("initial_margin_on_sell")]
    public double InitialMarginOnSell { get; set; }

    /// <summary>
    /// Стоимость шага цены
    /// </summary>
    [Column("min_price_increment_amount")]
    public double MinPriceIncrementAmount { get; set; }
}