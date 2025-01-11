namespace Oid85.FinMarket.Domain.Models;

public class Multiplicator
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
    /// Уникальный идентификатор инструмента
    /// </summary>
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Рыночная капитализация
    /// </summary>
    public double MarketCapitalization { get; set; }
    
    /// <summary>
    /// Годовой минимум
    /// </summary>
    public double LowOfYear { get; set; }
    
    /// <summary>
    /// Годовой максимум
    /// </summary>
    public double HighOfYear { get; set; }
    
    /// <summary>
    /// Бета-коэффициент
    /// </summary>
    public double Beta { get; set; }
    
    /// <summary>
    /// Чистая прибыль
    /// </summary>
    public double NetIncome { get; set; }
    
    /// <summary>
    /// EBITDA
    /// </summary>
    public double Ebitda { get; set; }
    
    /// <summary>
    /// Прибыль на акцию
    /// </summary>
    public double Eps { get; set; }
    
    /// <summary>
    /// Свободный денежный поток
    /// </summary>
    public double FreeCashFlow { get; set; }
    
    /// <summary>
    /// EV / EBITDA
    /// </summary>
    public double EvToEbitda { get; set; }
    
    /// <summary>
    /// Долг / EBITDA
    /// </summary>
    public double TotalDebtToEbitda { get; set; }
    
    /// <summary>
    /// Чистый долг / EBITDA
    /// </summary>
    public double NetDebtToEbitda { get; set; }
}