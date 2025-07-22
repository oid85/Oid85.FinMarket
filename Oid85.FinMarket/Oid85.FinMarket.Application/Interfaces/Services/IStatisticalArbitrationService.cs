namespace Oid85.FinMarket.Application.Interfaces.Services;

public interface IStatisticalArbitrationService
{
    /// <summary>
    /// Рассчитать корреляции
    /// </summary>
    Task CalculateCorrelationAsync();
    
    /// <summary>
    /// Рассчитать хвосты регрессии
    /// </summary>
    Task CalculateRegressionTailsAsync();
}