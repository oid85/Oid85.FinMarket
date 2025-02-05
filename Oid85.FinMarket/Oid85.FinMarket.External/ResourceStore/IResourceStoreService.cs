using Oid85.FinMarket.External.ResourceStore.Models;

namespace Oid85.FinMarket.External.ResourceStore;

/// <summary>
/// Сервис ресурсов
/// </summary>
public interface IResourceStoreService
{
    /// <summary>
    /// Получить тикеры акций из списка наблюдения
    /// </summary>
    Task<List<string>> GetSharesWatchlistAsync();
    
    /// <summary>
    /// Получить тикеры облигаций из списка наблюдения
    /// </summary>
    Task<List<string>> GetBondsWatchlistAsync();
    
    /// <summary>
    /// Получить тикеры фьючерсов из списка наблюдения
    /// </summary>
    Task<List<string>> GetFuturesWatchlistAsync();
    
    /// <summary>
    /// Получить тикеры валют из списка наблюдения
    /// </summary>
    Task<List<string>> GetCurrenciesWatchlistAsync();
    
    /// <summary>
    /// Получить тикеры индексов из списка наблюдения
    /// </summary>
    Task<List<string>> GetIndexesWatchlistAsync();
    
    /// <summary>
    /// Получить тикеры индекса Московской биржи
    /// </summary>
    Task<List<string>> GetIMoexTickersAsync();
    
    /// <summary>
    /// Получить мультипликатор
    /// </summary>
    Task<List<MultiplicatorResource>> GetMultiplicatorsLtmAsync();
    
    /// <summary>
    /// Получить ценовые уровни
    /// </summary>
    Task<List<PriceLevelResource>> GetPriceLevelsAsync(string ticker);
    
    /// <summary>
    /// Получить коды фьючерсов по месяцам
    /// </summary>
    Task<List<FutureCodeResource>> GetFutureCodesAsync();
    
    /// <summary>
    /// Получить значения ключевой ставки ЦБ
    /// </summary>
    Task<List<DateValueResource<double>>> GetKeyRatesAsync();
    
    /// <summary>
    /// Получить параметры спредов
    /// </summary>
    Task<List<SpreadResource>> GetSpreadsAsync();
    
    /// <summary>
    /// Получить цветовую палитру для AggregatedAnalyse
    /// </summary>
    Task<List<ValueColorResource<int>>> GetColorPaletteAggregatedAnalyseAsync();
    
    /// <summary>
    /// Получить цветовую палитру для CandleSequence
    /// </summary>
    Task<List<ValueColorResource<string>>> GetColorPaletteCandleSequenceAsync();
    
    /// <summary>
    /// Получить цветовую палитру для RsiInterpretation
    /// </summary>
    Task<List<ValueColorResource<string>>> GetColorPaletteRsiInterpretationAsync();
    
    /// <summary>
    /// Получить цветовую палитру для TrendDirection
    /// </summary>
    Task<List<ValueColorResource<string>>> GetColorPaletteTrendDirectionAsync();
    
    /// <summary>
    /// Получить цветовую палитру для VolumeDirection
    /// </summary>
    Task<List<ValueColorResource<string>>> GetColorPaletteVolumeDirectionAsync();
    
    /// <summary>
    /// Получить цветовую палитру для YieldDividend
    /// </summary>
    Task<List<RangeColorResource>> GetColorPaletteYieldDividendAsync();
    
    /// <summary>
    /// Получить цветовую палитру для YieldCoupon
    /// </summary>
    Task<List<RangeColorResource>> GetColorPaletteYieldCouponAsync();
    
    /// <summary>
    /// Получить цветовую палитру для YieldLtm
    /// </summary>
    Task<List<RangeColorResource>> GetColorPaletteYieldLtmAsync();
    
    /// <summary>
    /// Получить цветовую палитру для PE
    /// </summary>
    Task<List<RangeColorResource>> GetColorPalettePeAsync();
    
    /// <summary>
    /// Получить цветовую палитру для EV / EBITDA
    /// </summary>
    Task<List<RangeColorResource>> GetColorPaletteEvToEbitdaAsync();
    
    /// <summary>
    /// Получить цветовую палитру для NetDebt / EBITDA
    /// </summary>
    Task<List<RangeColorResource>> GetColorPaletteNetDebtToEbitdaAsync();
    
    /// <summary>
    /// Получить цветовую палитру для SpreadPricePosition
    /// </summary>
    Task<List<ValueColorResource<string>>> GetColorPaletteSpreadPricePositionAsync();
    
    /// <summary>
    /// Получить цветовую палитру для ForecastRecommendation
    /// </summary>
    Task<List<ValueColorResource<string>>> GetColorPaletteForecastRecommendationAsync();
    
    /// <summary>
    /// Получить фильтр для облигаций
    /// </summary>
    Task<FilterBondsResource?> GetFilterBondsResourceAsync();
}