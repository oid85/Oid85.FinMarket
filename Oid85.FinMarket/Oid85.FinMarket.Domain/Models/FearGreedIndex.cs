namespace Oid85.FinMarket.Domain.Models;

/// <summary>
/// Индекс силы и жадности
/// </summary>
public class FearGreedIndex
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Дата
    /// </summary>
    public DateOnly Date { get; set; }
    
    /// <summary>
    /// Рыночный моментум (Market Momentum)
    /// </summary>
    public double MarketMomentum { get; set; }
    
    /// <summary>
    /// Волатильность рынка (Market Volatility)
    /// </summary>
    public double MarketVolatility { get; set; }
    
    /// <summary>
    /// Ширина рынка (Stock Price Breadth)
    /// </summary>
    public double StockPriceBreadth { get; set; }
    
    /// <summary>
    /// Сила цен акций (Stock Price Strength)
    /// </summary>
    public double StockPriceStrength { get; set; }
    
    /// <summary>
    /// Индекс силы и жадности (Fear Greed Index)
    /// </summary>
    public double Value { get; set; }
}