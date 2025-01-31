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
    /// Количество обыкновенных акций
    /// </summary>
    public double TotalSharesAo { get; set; }
    
    /// <summary>
    /// Количество привелегированных акций
    /// </summary>
    public double TotalSharesAp { get; set; }
    
    /// <summary>
    /// Бета-коэффициент
    /// </summary>
    public double Beta { get; set; }
    
    /// <summary>
    /// Выручка
    /// </summary>
    public double Revenue { get; set; }
    
    /// <summary>
    /// Операционная прибыль
    /// </summary>
    public double OperatingIncome { get; set; }
    
    /// <summary>
    /// P/E
    /// </summary>
    public double Pe { get; set; }
    
    /// <summary>
    /// P/B
    /// </summary>
    public double Pb { get; set; }
    
    /// <summary>
    /// P/BV
    /// </summary>
    public double Pbv { get; set; }
    
    /// <summary>
    /// EV
    /// </summary>
    public double Ev { get; set; }
    
    /// <summary>
    /// ROE
    /// </summary>
    public double Roe { get; set; }
    
    /// <summary>
    /// ROA
    /// </summary>
    public double Roa { get; set; }
    
    /// <summary>
    /// Чистая процентная маржа
    /// </summary>
    public double NetInterestMargin { get; set; }
    
    /// <summary>
    /// Долг
    /// </summary>
    public double TotalDebt { get; set; }
    
    /// <summary>
    /// Чистый долг
    /// </summary>
    public double NetDebt { get; set; }
    
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