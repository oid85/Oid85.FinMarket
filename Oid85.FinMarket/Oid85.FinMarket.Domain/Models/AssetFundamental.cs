namespace Oid85.FinMarket.Domain.Models;

public class AssetFundamental
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
    /// Уникальный идентификатор актива
    /// </summary>
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Валюта
    /// </summary>
    public string Currency { get; set; } = string.Empty;
    
    /// <summary>
    /// Рыночная капитализация
    /// </summary>
    public double MarketCapitalization { get; set; }
    
    /// <summary>
    /// Максимум за год
    /// </summary>
    public double HighPriceLast52Weeks { get; set; }
    
    /// <summary>
    /// Минимум за год
    /// </summary>
    public double LowPriceLast52Weeks { get; set; }
    
    /// <summary>
    /// Средний объём торгов за 10 дней
    /// </summary>
    public double AverageDailyVolumeLast10Days { get; set; }
    
    /// <summary>
    /// Средний объём торгов за месяц
    /// </summary>
    public double AverageDailyVolumeLast4Weeks { get; set; }
    
    /// <summary>
    /// Бета-коэффициент
    /// </summary>
    public double Beta { get; set; }
    
    /// <summary>
    /// Доля акций в свободном обращении
    /// </summary>
    public double FreeFloat { get; set; }
    
    /// <summary>
    /// Процент форвардной дивидендной доходности по отношению к цене акций
    /// </summary>
    public double ForwardAnnualDividendYield { get; set; }
    
    /// <summary>
    /// Количество акций в обращении
    /// </summary>
    public double SharesOutstanding { get; set; }
    
    /// <summary>
    /// Выручка
    /// </summary>
    public double RevenueTtm { get; set; }
    
    /// <summary>
    /// EBITDA — прибыль до вычета процентов, налогов, износа и амортизации
    /// </summary>
    public double EbitdaTtm { get; set; }
    
    /// <summary>
    /// Чистая прибыль
    /// </summary>
    public double NetIncomeTtm { get; set; }
    
    /// <summary>
    /// EPS — величина чистой прибыли компании,
    /// которая приходится на каждую обыкновенную акцию
    /// </summary>
    public double EpsTtm { get; set; }
    
    /// <summary>
    /// EPS компании с допущением, что все конвертируемые
    /// ценные бумаги компании были сконвертированы в обыкновенные акции
    /// </summary>
    public double DilutedEpsTtm { get; set; }
    
    /// <summary>
    /// Свободный денежный поток
    /// </summary>
    public double FreeCashFlowTtm { get; set; }
    
    /// <summary>
    /// Среднегодовой рocт выручки за 5 лет
    /// </summary>
    public double FiveYearAnnualRevenueGrowthRate { get; set; }
    
    /// <summary>
    /// Среднегодовой рocт выручки за 3 года
    /// </summary>
    public double ThreeYearAnnualRevenueGrowthRate { get; set; }
    
    /// <summary>
    /// Соотношение рыночной капитализации компании к её чистой прибыли
    /// </summary>
    public double PeRatioTtm { get; set; }
    
    /// <summary>
    /// Соотношение рыночной капитализации компании к её выручке
    /// </summary>
    public double PriceToSalesTtm { get; set; }
    
    /// <summary>
    /// Соотношение рыночной капитализации компании к её балансовой стоимости
    /// </summary>
    public double PriceToBookTtm { get; set; }
    
    /// <summary>
    /// Соотношение рыночной капитализации компании к её свободному денежному потоку
    /// </summary>
    public double PriceToFreeCashFlowTtm { get; set; }
    
    /// <summary>
    /// Рыночная стоимость компании
    /// </summary>
    public double TotalEnterpriseValueMrq { get; set; }
    
    /// <summary>
    /// Соотношение EV и EBITDA
    /// </summary>
    public double EvToEbitdaMrq { get; set; }
        
    /// <summary>
    /// Маржа чистой прибыли
    /// </summary>
    public double NetMarginMrq { get; set; }
    
    /// <summary>
    /// Рентабельность чистой прибыли
    /// </summary>
    public double NetInterestMarginMrq { get; set; }
        
    /// <summary>
    /// Рентабельность собственного капитала
    /// </summary>
    public double Roe { get; set; }
    
    /// <summary>
    /// Рентабельность активов
    /// </summary>
    public double Roa { get; set; }
        
    /// <summary>
    /// Рентабельность активов
    /// </summary>
    public double Roic { get; set; }
    
    /// <summary>
    /// Сумма краткосрочных и долгосрочных обязательств компании
    /// </summary>
    public double TotalDebtMrq { get; set; }
        
    /// <summary>
    /// Соотношение долга к собственному капиталу
    /// </summary>
    public double TotalDebtToEquityMrq { get; set; }
    
    /// <summary>
    /// Total Debt/EBITDA
    /// </summary>
    public double TotalDebtToEbitdaMrq { get; set; }
        
    /// <summary>
    /// Отношение свободногоо кэша к стоимости
    /// </summary>
    public double FreeCashFlowToPrice { get; set; }
    
    /// <summary>
    /// Отношение чистого долга к EBITDA
    /// </summary>
    public double NetDebtToEbitda { get; set; }
        
    /// <summary>
    /// Коэффициент текущей ликвидности
    /// </summary>
    public double CurrentRatioMrq { get; set; }
    
    /// <summary>
    /// Коэффициент покрытия фиксированных платежей — FCCR
    /// </summary>
    public double FixedChargeCoverageRatioFy { get; set; }
        
    /// <summary>
    /// Дивидендная доходность за 12 месяцев
    /// </summary>
    public double DividendYieldDailyTtm { get; set; }
    
    /// <summary>
    /// Выплаченные дивиденды за 12 месяцев
    /// </summary>
    public double DividendRateTtm { get; set; }
        
    /// <summary>
    /// Значение дивидендов на акцию
    /// </summary>
    public double DividendsPerShare { get; set; }
    
    /// <summary>
    /// Средняя дивидендная доходность за 5 лет
    /// </summary>
    public double FiveYearsAverageDividendYield { get; set; }
        
    /// <summary>
    /// Среднегодовой рост дивидендов за 5 лет
    /// </summary>
    public double FiveYearAnnualDividendGrowthRate { get; set; }
    
    /// <summary>
    /// Процент чистой прибыли, уходящий на выплату дивидендов
    /// </summary>
    public double DividendPayoutRatioFy { get; set; }
        
    /// <summary>
    /// Деньги, потраченные на обратный выкуп акций
    /// </summary>
    public double BuyBackTtm { get; set; }
    
    /// <summary>
    /// Рост выручки за 1 год
    /// </summary>
    public double OneYearAnnualRevenueGrowthRate { get; set; }
        
    /// <summary>
    /// Код страны
    /// </summary>
    public string DomicileIndicatorCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Соотношение депозитарной расписки к акциям
    /// </summary>
    public double AdrToCommonShareRatio { get; set; }
        
    /// <summary>
    /// Количество сотрудников
    /// </summary>
    public double NumberOfEmployees { get; set; }
    
    /// <summary>
    /// ex_dividend_date
    /// </summary>
    public DateOnly ExDividendDate { get; set; }
        
    /// <summary>
    /// Начало фискального периода
    /// </summary>
    public DateOnly FiscalPeriodStartDate { get; set; }
    
    /// <summary>
    /// Окончание фискального периода
    /// </summary>
    public DateOnly FiscalPeriodEndDate { get; set; }
        
    /// <summary>
    /// Изменение общего дохода за 5 лет
    /// </summary>
    public double RevenueChangeFiveYears { get; set; }
    
    /// <summary>
    /// Изменение EPS за 5 лет
    /// </summary>
    public double EpsChangeFiveYears { get; set; }
        
    /// <summary>
    /// Изменение EBIDTA за 5 лет
    /// </summary>
    public double EbitdaChangeFiveYears { get; set; }
    
    /// <summary>
    /// Изменение общей задолжности за 5 лет
    /// </summary>
    public double TotalDebtChangeFiveYears { get; set; }
        
    /// <summary>
    /// Отношение EV к выручке
    /// </summary>
    public double EvToSales { get; set; }
}