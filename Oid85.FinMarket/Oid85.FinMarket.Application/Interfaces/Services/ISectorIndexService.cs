namespace Oid85.FinMarket.Application.Interfaces.Services;

public interface ISectorIndexService
{
    /// <summary>
    /// Расчет свечей индекса сектора Нефтегаз
    /// </summary>
    /// <returns></returns>
    Task CalculateOilAndGasSectorIndexDailyCandlesAsync();
    
    /// <summary>
    /// Расчет свечей индекса сектора Банки
    /// </summary>
    /// <returns></returns>
    Task CalculateBanksSectorIndexDailyCandlesAsync();
    
    /// <summary>
    /// Расчет свечей индекса сектора Энергетики
    /// </summary>
    /// <returns></returns>
    Task CalculateEnergSectorIndexDailyCandlesAsync();
    
    /// <summary>
    /// Расчет свечей индекса сектора Финансы
    /// </summary>
    /// <returns></returns>
    Task CalculateFinanceSectorIndexDailyCandlesAsync();
    
    /// <summary>
    /// Расчет свечей индекса сектора ЖКХ
    /// </summary>
    /// <returns></returns>
    Task CalculateHousingAndUtilitiesSectorIndexDailyCandlesAsync();
    
    /// <summary>
    /// Расчет свечей индекса сектора Черная металлургия
    /// </summary>
    /// <returns></returns>
    Task CalculateIronAndSteelIndustrySectorIndexDailyCandlesAsync();
    
    /// <summary>
    /// Расчет свечей индекса сектора ИТ
    /// </summary>
    /// <returns></returns>
    Task CalculateItSectorIndexDailyCandlesAsync();
    
    /// <summary>
    /// Расчет свечей индекса сектора Добыча
    /// </summary>
    /// <returns></returns>
    Task CalculateMiningSectorIndexDailyCandlesAsync();
    
    /// <summary>
    /// Расчет свечей индекса сектора Цветная металлургия
    /// </summary>
    /// <returns></returns>
    Task CalculateNonFerrousMetallurgySectorIndexDailyCandlesAsync();
    
    /// <summary>
    /// Расчет свечей индекса сектора Потребительский
    /// </summary>
    /// <returns></returns>
    Task CalculateRetailSectorIndexDailyCandlesAsync();
    
    /// <summary>
    /// Расчет свечей индекса сектора Телеком
    /// </summary>
    /// <returns></returns>
    Task CalculateTelecomSectorIndexDailyCandlesAsync();
}