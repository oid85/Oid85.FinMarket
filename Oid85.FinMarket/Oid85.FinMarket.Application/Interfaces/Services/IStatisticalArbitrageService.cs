using Oid85.FinMarket.Domain.Models.StatisticalArbitration;

namespace Oid85.FinMarket.Application.Interfaces.Services;

public interface IStatisticalArbitrageService
{
    /// <summary>
    /// Рассчитать корреляции
    /// </summary>
    Task CalculateCorrelationAsync();
    
    /// <summary>
    /// Рассчитать хвосты регрессии
    /// </summary>
    Task<Dictionary<string, List<RegressionTail>>> CalculateRegressionTailsAsync();
}