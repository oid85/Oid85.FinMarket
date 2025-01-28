namespace Oid85.FinMarket.Domain.Models;

public class Future
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
    /// Нижний целевой уровень
    /// </summary>
    public double HighTargetPrice { get; set; }
    
    /// <summary>
    /// Верхний целевой уровень
    /// </summary>
    public double LowTargetPrice { get; set; }
    
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
    /// Дата истечения срока
    /// </summary>
    public DateOnly ExpirationDate = DateOnly.MinValue;

    /// <summary>
    /// Лотность инструмента
    /// </summary>
    public int Lot { get; set; } = 1;

    /// <summary>
    /// Дата начала обращения контракта по UTC
    /// </summary>
    public DateOnly FirstTradeDate { get; set; }

    /// <summary>
    /// Дата по UTC, до которой возможно проведение операций с фьючерсом
    /// </summary>
    public DateOnly LastTradeDate { get; set; }

    /// <summary>
    /// Тип фьючерса
    /// </summary>
    public string FutureType { get; set; } = string.Empty;

    /// <summary>
    /// Тип актива
    /// </summary>
    public string AssetType { get; set; } = string.Empty;

    /// <summary>
    /// Основной актив
    /// </summary>
    public string BasicAsset { get; set; } = string.Empty;

    /// <summary>
    /// Размер основного актива
    /// </summary>
    public double BasicAssetSize { get; set; }

    /// <summary>
    /// Гарантийное обеспечение при покупке
    /// </summary>
    public double InitialMarginOnBuy { get; set; }

    /// <summary>
    /// Гарантийное обеспечение при продаже
    /// </summary>
    public double InitialMarginOnSell { get; set; }

    /// <summary>
    /// Стоимость шага цены
    /// </summary>
    public double MinPriceIncrementAmount { get; set; }
}