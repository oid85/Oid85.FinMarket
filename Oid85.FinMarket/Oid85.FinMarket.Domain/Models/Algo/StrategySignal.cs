namespace Oid85.FinMarket.Domain.Models.Algo;

public class StrategySignal
{
    /// <summary>
    /// Тикер инструмента
    /// </summary>
    public string Ticker { get; set; } = string.Empty; 
    
    /// <summary>
    /// Количество сигналов
    /// </summary>
    public int CountSignals { get; set; }

    /// <summary>
    /// Размер позиции
    /// </summary>
    public double PositionCost { get; set; }
    
    /// <summary>
    /// Размер позиции, шт
    /// </summary>
    public int PositionSize { get; set; } 
    
    /// <summary>
    /// Цена инструмента
    /// </summary>
    public double LastPrice { get; set; } 
}