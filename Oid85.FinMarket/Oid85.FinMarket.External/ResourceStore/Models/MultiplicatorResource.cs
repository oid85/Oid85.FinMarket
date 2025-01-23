namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Мультипликатор
/// </summary>
public class MultiplicatorResource
{
    /// <summary>
    /// Количество акций обыкновенных
    /// </summary>
    public ValueUnitResource TotalSharesAo { get; set; } = new();
    
    /// <summary>
    /// Количество акций привелегированных
    /// </summary>
    public ValueUnitResource TotalSharesAp { get; set; } = new();
    
    /// <summary>
    /// Выручка
    /// </summary>
    public ValueUnitResource Revenue { get; set; } = new();
    
    /// <summary>
    /// Чистая прибыль
    /// </summary>
    public ValueUnitResource NetIncome { get; set; } = new();
    
    /// <summary>
    /// Операционная прибыль
    /// </summary>
    public ValueUnitResource OperatingIncome { get; set; } = new();
    
    /// <summary>
    /// EBITDA
    /// </summary>
    public ValueUnitResource Ebitda { get; set; } = new();
    
    /// <summary>
    /// P/E
    /// </summary>
    public ValueUnitResource Pe { get; set; } = new();
    
    /// <summary>
    /// P/B
    /// </summary>
    public ValueUnitResource Pb { get; set; } = new();
    
    /// <summary>
    /// P/BV
    /// </summary>
    public ValueUnitResource Pv { get; set; } = new();
    
    /// <summary>
    /// EV
    /// </summary>
    public ValueUnitResource Ev { get; set; } = new();
    
    /// <summary>
    /// ROE
    /// </summary>
    public ValueUnitResource Roe { get; set; } = new();
    
    /// <summary>
    /// ROA
    /// </summary>
    public ValueUnitResource Roa { get; set; } = new();
    
    /// <summary>
    /// EPS
    /// </summary>
    public ValueUnitResource Eps { get; set; } = new();
    
    /// <summary>
    /// Чистая процентная маржа
    /// </summary>
    public ValueUnitResource NetInterestMargin { get; set; } = new();
    
    /// <summary>
    /// Общий долг
    /// </summary>
    public ValueUnitResource TotalDebt { get; set; } = new();

    /// <summary>
    /// Чистый долг
    /// </summary>
    public ValueUnitResource NetDebt { get; set; } = new();
}
