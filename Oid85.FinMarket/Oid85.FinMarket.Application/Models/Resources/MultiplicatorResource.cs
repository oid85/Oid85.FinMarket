namespace Oid85.FinMarket.Application.Models.Resources;

/// <summary>
/// Мультипликатор
/// </summary>
public class MultiplicatorResource
{
    /// <summary>
    /// Тикер
    /// </summary>
    public string Ticker { get; set; } = string.Empty;
    
    /// <summary>
    /// Количество акций обыкновенных
    /// </summary>
    public int TotalSharesAo { get; set; }
    
    /// <summary>
    /// Количество акций привелегированных
    /// </summary>
    public int TotalSharesAp { get; set; }
    
    /// <summary>
    /// Выручка
    /// </summary>
    public double Revenue { get; set; }
    
    /// <summary>
    /// Чистая прибыль
    /// </summary>
    public double NetIncome { get; set; }
    
    /// <summary>
    /// Операционная прибыль
    /// </summary>
    public double OperatingIncome { get; set; }
    
    /// <summary>
    /// EBITDA
    /// </summary>
    public double Ebitda { get; set; }
    
    /// <summary>
    /// P/E
    /// </summary>
    public double Pe { get; set; }
    
    /// <summary>
    /// P/B
    /// </summary>
    public double Pb { get; set; }
    
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
    /// EPS
    /// </summary>
    public double Eps { get; set; }
    
    /// <summary>
    /// Чистая процентная маржа
    /// </summary>
    public double NetInterestMargin { get; set; }
    
    /// <summary>
    /// Общий долг
    /// </summary>
    public double TotalDebt { get; set; }
    
    /// <summary>
    /// Чистый долг
    /// </summary>
    public double NetDebt { get; set; }
    
    /// <summary>
    /// Прогноз див. доходности ао
    /// </summary>
    public double ForecastDividendAo { get; set; }
    
    /// <summary>
    /// Прогноз див. доходности ап
    /// </summary>
    public double ForecastDividendAp { get; set; }
}
