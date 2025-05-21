namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис анализа
/// </summary>
public interface IAnalyseService
{
    /// <summary>
    /// Анализ акций
    /// </summary>
    Task<bool> DailyAnalyseSharesAsync();
    
    /// <summary>
    /// Анализ облигаций
    /// </summary>
    Task<bool> DailyAnalyseBondsAsync();
    
    /// <summary>
    /// Анализ валют
    /// </summary>
    Task<bool> DailyAnalyseCurrenciesAsync();
    
    /// <summary>
    /// Анализ фьючерсов
    /// </summary>
    Task<bool> DailyAnalyseFuturesAsync();
    
    /// <summary>
    /// Анализ индексов
    /// </summary>
    Task<bool> DailyAnalyseIndexesAsync();
    
    /// <summary>
    /// Анализ сектора Нефтегаз
    /// </summary>
    Task<bool> DailyAnalyseOilAndGasSectorIndexAsync();
    
    /// <summary>
    /// Анализ сектора Банки
    /// </summary>
    Task<bool> DailyAnalyseBanksSectorIndexAsync();
    
    /// <summary>
    /// Анализ сектора Энергетика
    /// </summary>
    Task<bool> DailyAnalyseEnergSectorIndexAsync();
    
    /// <summary>
    /// Анализ сектора Финансы
    /// </summary>
    Task<bool> DailyAnalyseFinanceSectorIndexAsync();
    
    /// <summary>
    /// Анализ сектора ЖКХ
    /// </summary>
    Task<bool> DailyAnalyseHousingAndUtilitiesSectorIndexAsync();
    
    /// <summary>
    /// Анализ сектора Черная металлургия
    /// </summary>
    Task<bool> DailyAnalyseIronAndSteelIndustrySectorIndexAsync();
    
    /// <summary>
    /// Анализ сектора ИТ
    /// </summary>
    Task<bool> DailyAnalyseItSectorIndexAsync();
    
    /// <summary>
    /// Анализ сектора Добыча
    /// </summary>
    Task<bool> DailyAnalyseMiningSectorIndexAsync();
    
    /// <summary>
    /// Анализ сектора Цветная металлургия
    /// </summary>
    Task<bool> DailyAnalyseNonFerrousMetallurgySectorIndexAsync();
    
    /// <summary>
    /// Анализ сектора Потребительский
    /// </summary>
    Task<bool> DailyAnalyseRetailSectorIndexAsync();
    
    /// <summary>
    /// Анализ сектора Телеком
    /// </summary>
    Task<bool> DailyAnalyseTelecomSectorIndexAsync();
}