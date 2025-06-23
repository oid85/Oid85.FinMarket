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
}