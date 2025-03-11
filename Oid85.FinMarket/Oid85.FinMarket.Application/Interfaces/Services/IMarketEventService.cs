namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис событий рынка
/// </summary>
public interface IMarketEventService
{
    /// <summary>
    /// Расчет рыночного события Супертренд (вверх)
    /// </summary>
    Task CheckSupertrendUpMarketEventAsync();

    /// <summary>
    /// Расчет рыночного события Супертренд (вниз)
    /// </summary>
    Task CheckSupertrendDownMarketEventAsync();
    
    /// <summary>
    /// Расчет рыночного события Растущий объем
    /// </summary>
    Task CheckCandleVolumeUpMarketEventAsync();

    /// <summary>
    /// Расчет рыночного события Свечи одного цвета (белые)
    /// </summary>
    Task CheckCandleSequenceWhiteMarketEventAsync();

    /// <summary>
    /// Расчет рыночного события Свечи одного цвета (черные)
    /// </summary>
    Task CheckCandleSequenceBlackMarketEventAsync();
    
    /// <summary>
    /// Расчет рыночного события RSI (вход в зону перекупленности)
    /// </summary>
    Task CheckRsiOverBoughtInputMarketEventAsync();
    
    /// <summary>
    /// Расчет рыночного события RSI (выход из зоны перекупленности)
    /// </summary>
    Task CheckRsiOverBoughtOutputMarketEventAsync();
    
    /// <summary>
    /// Расчет рыночного события RSI (вход в зону перепроданности)
    /// </summary>
    Task CheckRsiOverOverSoldInputMarketEventAsync();
    
    /// <summary>
    /// Расчет рыночного события RSI (выход из зоны перепроданности)
    /// </summary>
    Task CheckRsiOverOverSoldOutputMarketEventAsync();
    
    /// <summary>
    /// Расчет рыночного события Пересечение ценой уровня
    /// </summary>
    Task CheckCrossPriceLevelMarketEventAsync();
    
    /// <summary>
    /// Расчет рыночного события Спред превышает 1 %
    /// </summary>
    Task CheckSpreadGreaterPercent1MarketEventAsync();
    
    /// <summary>
    /// Расчет рыночного события Спред превышает 2 %
    /// </summary>
    Task CheckSpreadGreaterPercent2MarketEventAsync();
    
    /// <summary>
    /// Расчет рыночного события Спред превышает 3 %
    /// </summary>
    Task CheckSpreadGreaterPercent3MarketEventAsync();
    
    /// <summary>
    /// Расчет рыночного события Реализация прогноза
    /// </summary>
    Task CheckForecastReleasedMarketEventAsync();
    
    /// <summary>
    /// Расчет рыночного события Ударный день
    /// </summary>
    Task CheckStrikeDayMarketEventAsync();
}