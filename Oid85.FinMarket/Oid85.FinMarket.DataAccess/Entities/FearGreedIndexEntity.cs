using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class FearGreedIndexEntity : BaseEntity
{
    /// <summary>
    /// Дата
    /// </summary>
    [Column("date", TypeName = "date")]
    public DateOnly Date { get; set; }
    
    /// <summary>
    /// Рыночный моментум (Market Momentum)
    /// </summary>
    [Column("market_momentum")]
    public double MarketMomentum { get; set; }
    
    /// <summary>
    /// Волатильность рынка (Market Volatility)
    /// </summary>
    [Column("market_volatility")]
    public double MarketVolatility { get; set; }
    
    /// <summary>
    /// Ширина рынка (Stock Price Breadth)
    /// </summary>
    [Column("stock_price_breadth")]
    public double StockPriceBreadth { get; set; }
    
    /// <summary>
    /// Сила цен акций (Stock Price Strength)
    /// </summary>
    [Column("stock_price_strength")]
    public double StockPriceStrength { get; set; }
    
    /// <summary>
    /// Индекс силы и жадности (Fear Greed Index)
    /// </summary>
    [Column("value")]
    public double Value { get; set; }
}