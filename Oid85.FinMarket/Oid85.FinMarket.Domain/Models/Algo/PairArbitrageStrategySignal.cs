namespace Oid85.FinMarket.Domain.Models.Algo;

public class PairArbitrageStrategySignal
{
    public PairArbitrageStrategySignal()
    {
        
    }
    
    public PairArbitrageStrategySignal(string tickerFirst, string tickerSecond)
    {
        TickerFirst = tickerFirst;
        TickerSecond = tickerSecond;
        CountStrategies = 0;
        CountSignals = 0;
        PercentSignals = 0;
        LastPriceFirst = 0.0;
        LastPriceSecond = 0.0;
        PositionCost = 0.0;
        PositionSizeFirst = 0;
        PositionSizeSecond = 0;
        PositionPercentPortfolio = 0;
    }
    
    /// <summary>
    /// Тикер инструмента
    /// </summary>
    public string TickerFirst { get; set; } = string.Empty;
    
    /// <summary>
    /// Тикер инструмента
    /// </summary>
    public string TickerSecond { get; set; } = string.Empty;
    
    /// <summary>
    /// Количество сигналов
    /// </summary>
    public int CountSignals { get; set; }
    
    /// <summary>
    /// Количество стратегий
    /// </summary>
    public int CountStrategies { get; set; }    
    
    /// <summary>
    /// Процент сигналов
    /// </summary>
    public double PercentSignals { get; set; }    
    
    /// <summary>
    /// Размер позиции, руб
    /// </summary>
    public double PositionCost { get; set; }   
    
    /// <summary>
    /// Размер позиции, шт
    /// </summary>
    public int PositionSizeFirst { get; set; } 
    
    /// <summary>
    /// Размер позиции, шт
    /// </summary>
    public int PositionSizeSecond { get; set; } 
    
    /// <summary>
    /// Размер позиции в процентах от портфеля
    /// </summary>
    public double PositionPercentPortfolio { get; set; }  
    
    /// <summary>
    /// Цена инструмента
    /// </summary>
    public double LastPriceFirst { get; set; } 
    
    /// <summary>
    /// Цена инструмента
    /// </summary>
    public double LastPriceSecond { get; set; } 
}