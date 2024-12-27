using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class AssetFundamentalEntity : AuditableEntity
{
    /// <summary>
    /// Дата
    /// </summary>
    [Column("date", TypeName = "date")]
    public DateOnly Date { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор актива
    /// </summary>
    [Column("instrument_id")]
    public Guid InstrumentId { get; set; }
    
    /// <summary>
    /// Валюта
    /// </summary>
    [Column("currency")]
    public string Currency { get; set; } = string.Empty;
    
    /// <summary>
    /// Рыночная капитализация
    /// </summary>
    [Column("market_capitalization")]
    public double MarketCapitalization { get; set; }
    
    /// <summary>
    /// Максимум за год
    /// </summary>
    [Column("high_price_last_52_weeks")]
    public double HighPriceLast52Weeks { get; set; }
    
    /// <summary>
    /// Минимум за год
    /// </summary>
    [Column("low_price_last_52_weeks")]
    public double LowPriceLast52Weeks { get; set; }
    
    /// <summary>
    /// Средний объём торгов за 10 дней
    /// </summary>
    [Column("average_daily_volume_last_10_days")]
    public double AverageDailyVolumeLast10Days { get; set; }
    
    /// <summary>
    /// Средний объём торгов за месяц
    /// </summary>
    [Column("average_daily_volume_last_4_weeks")]
    public double AverageDailyVolumeLast4Weeks { get; set; }
    
    /// <summary>
    /// Бета-коэффициент
    /// </summary>
    [Column("beta")]
    public double Beta { get; set; }
    
    /// <summary>
    /// Доля акций в свободном обращении
    /// </summary>
    [Column("free_float")]
    public double FreeFloat { get; set; }
    
    /// <summary>
    /// Процент форвардной дивидендной доходности по отношению к цене акций
    /// </summary>
    [Column("forward_annual_dividend_yield")]
    public double ForwardAnnualDividendYield { get; set; }
    
    /// <summary>
    /// Количество акций в обращении
    /// </summary>
    [Column("shares_outstanding")]
    public double SharesOutstanding { get; set; }
    
    /// <summary>
    /// Выручка
    /// </summary>
    [Column("revenue_ttm")]
    public double RevenueTtm { get; set; }
    
    /// <summary>
    /// EBITDA — прибыль до вычета процентов, налогов, износа и амортизации
    /// </summary>
    [Column("ebitda_ttm")]
    public double EbitdaTtm { get; set; }
    
    /// <summary>
    /// Чистая прибыль
    /// </summary>
    [Column("net_income_ttm")]
    public double NetIncomeTtm { get; set; }
    
    /// <summary>
    /// EPS — величина чистой прибыли компании,
    /// которая приходится на каждую обыкновенную акцию
    /// </summary>
    [Column("eps_ttm")]
    public double EpsTtm { get; set; }
    
    /// <summary>
    /// EPS компании с допущением, что все конвертируемые
    /// ценные бумаги компании были сконвертированы в обыкновенные акции
    /// </summary>
    [Column("diluted_eps_ttm")]
    public double DilutedEpsTtm { get; set; }
    
    /// <summary>
    /// Свободный денежный поток
    /// </summary>
    [Column("free_cash_flow_ttm")]
    public double FreeCashFlowTtm { get; set; }
    
    /// <summary>
    /// Среднегодовой рocт выручки за 5 лет
    /// </summary>
    [Column("five_year_annual_revenue_growth_rate")]
    public double FiveYearAnnualRevenueGrowthRate { get; set; }
    
    /// <summary>
    /// Среднегодовой рocт выручки за 3 года
    /// </summary>
    [Column("three_year_annual_revenue_growth_rate")]
    public double ThreeYearAnnualRevenueGrowthRate { get; set; }
    
    /// <summary>
    /// Соотношение рыночной капитализации компании к её чистой прибыли
    /// </summary>
    [Column("pe_ratio_ttm")]
    public double PeRatioTtm { get; set; }
    
    /// <summary>
    /// Соотношение рыночной капитализации компании к её выручке
    /// </summary>
    [Column("price_to_sales_ttm")]
    public double PriceToSalesTtm { get; set; }
    
    /// <summary>
    /// Соотношение рыночной капитализации компании к её балансовой стоимости
    /// </summary>
    [Column("price_to_book_ttm")]
    public double PriceToBookTtm { get; set; }
    
    /// <summary>
    /// Соотношение рыночной капитализации компании к её свободному денежному потоку
    /// </summary>
    [Column("price_to_free_cash_flow_ttm")]
    public double PriceToFreeCashFlowTtm { get; set; }
    
    /// <summary>
    /// Рыночная стоимость компании
    /// </summary>
    [Column("total_enterprise_value_mrq")]
    public double TotalEnterpriseValueMrq { get; set; }
    
    /// <summary>
    /// Соотношение EV и EBITDA
    /// </summary>
    [Column("ev_to_ebitda_mrq")]
    public double EvToEbitdaMrq { get; set; }
        
    /// <summary>
    /// Маржа чистой прибыли
    /// </summary>
    [Column("net_margin_mrq")]
    public double NetMarginMrq { get; set; }
    
    /// <summary>
    /// Рентабельность чистой прибыли
    /// </summary>
    [Column("net_interest_margin_mrq")]
    public double NetInterestMarginMrq { get; set; }
        
    /// <summary>
    /// Рентабельность собственного капитала
    /// </summary>
    [Column("roe")]
    public double Roe { get; set; }
    
    /// <summary>
    /// Рентабельность активов
    /// </summary>
    [Column("roa")]
    public double Roa { get; set; }
        
    /// <summary>
    /// Рентабельность активов
    /// </summary>
    [Column("roic")]
    public double Roic { get; set; }
    
    /// <summary>
    /// Сумма краткосрочных и долгосрочных обязательств компании
    /// </summary>
    [Column("total_debt_mrq")]
    public double TotalDebtMrq { get; set; }
        
    /// <summary>
    /// Соотношение долга к собственному капиталу
    /// </summary>
    [Column("total_debt_to_equity_mrq")]
    public double TotalDebtToEquityMrq { get; set; }
    
    /// <summary>
    /// Total Debt/EBITDA
    /// </summary>
    [Column("total_debt_to_ebitda_mrq")]
    public double TotalDebtToEbitdaMrq { get; set; }
        
    /// <summary>
    /// Отношение свободногоо кэша к стоимости
    /// </summary>
    [Column("free_cash_flow_to_price")]
    public double FreeCashFlowToPrice { get; set; }
    
    /// <summary>
    /// Отношение чистого долга к EBITDA
    /// </summary>
    [Column("net_debt_to_ebitda")]
    public double NetDebtToEbitda { get; set; }
        
    /// <summary>
    /// Коэффициент текущей ликвидности
    /// </summary>
    [Column("current_ratio_mrq")]
    public double CurrentRatioMrq { get; set; }
    
    /// <summary>
    /// Коэффициент покрытия фиксированных платежей — FCCR
    /// </summary>
    [Column("fixed_charge_coverage_ratio_fy")]
    public double FixedChargeCoverageRatioFy { get; set; }
        
    /// <summary>
    /// Дивидендная доходность за 12 месяцев
    /// </summary>
    [Column("dividend_yield_daily_ttm")]
    public double DividendYieldDailyTtm { get; set; }
    
    /// <summary>
    /// Выплаченные дивиденды за 12 месяцев
    /// </summary>
    [Column("dividend_rate_ttm")]
    public double DividendRateTtm { get; set; }
        
    /// <summary>
    /// Значение дивидендов на акцию
    /// </summary>
    [Column("dividends_per_share")]
    public double DividendsPerShare { get; set; }
    
    /// <summary>
    /// Средняя дивидендная доходность за 5 лет
    /// </summary>
    [Column("five_years_average_dividend_yield")]
    public double FiveYearsAverageDividendYield { get; set; }
        
    /// <summary>
    /// Среднегодовой рост дивидендов за 5 лет
    /// </summary>
    [Column("five_year_annual_dividend_growth_rate")]
    public double FiveYearAnnualDividendGrowthRate { get; set; }
    
    /// <summary>
    /// Процент чистой прибыли, уходящий на выплату дивидендов
    /// </summary>
    [Column("dividend_payout_ratio_fy")]
    public double DividendPayoutRatioFy { get; set; }
        
    /// <summary>
    /// Деньги, потраченные на обратный выкуп акций
    /// </summary>
    [Column("buy_back_ttm")]
    public double BuyBackTtm { get; set; }
    
    /// <summary>
    /// Рост выручки за 1 год
    /// </summary>
    [Column("one_year_annual_revenue_growth_rate")]
    public double OneYearAnnualRevenueGrowthRate { get; set; }

    /// <summary>
    /// Код страны
    /// </summary>
    [Column("domicile_indicator_code"), MaxLength(10)]
    public string DomicileIndicatorCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Соотношение депозитарной расписки к акциям
    /// </summary>
    [Column("adr_to_common_share_ratio")]
    public double AdrToCommonShareRatio { get; set; }
        
    /// <summary>
    /// Количество сотрудников
    /// </summary>
    [Column("number_of_employees")]
    public double NumberOfEmployees { get; set; }
    
    /// <summary>
    /// ex_dividend_date
    /// </summary>
    [Column("ex_dividend_date", TypeName = "date")]
    public DateOnly ExDividendDate { get; set; }
        
    /// <summary>
    /// Начало фискального периода
    /// </summary>
    [Column("fiscal_period_start_date", TypeName = "date")]
    public DateOnly FiscalPeriodStartDate { get; set; }
    
    /// <summary>
    /// Окончание фискального периода
    /// </summary>
    [Column("fiscal_period_end_date", TypeName = "date")]
    public DateOnly FiscalPeriodEndDate { get; set; }
        
    /// <summary>
    /// Изменение общего дохода за 5 лет
    /// </summary>
    [Column("revenue_change_five_years")]
    public double RevenueChangeFiveYears { get; set; }
    
    /// <summary>
    /// Изменение EPS за 5 лет
    /// </summary>
    [Column("eps_change_five_years")]
    public double EpsChangeFiveYears { get; set; }
        
    /// <summary>
    /// Изменение EBIDTA за 5 лет
    /// </summary>
    [Column("ebitda_change_five_years")]
    public double EbitdaChangeFiveYears { get; set; }
    
    /// <summary>
    /// Изменение общей задолжности за 5 лет
    /// </summary>
    [Column("total_debt_change_five_years")]
    public double TotalDebtChangeFiveYears { get; set; }
        
    /// <summary>
    /// Отношение EV к выручке
    /// </summary>
    [Column("ev_to_sales")]
    public double EvToSales { get; set; }
}