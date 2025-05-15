namespace Oid85.FinMarket.Application.Interfaces.Services;

public interface ISectorIndexService
{
    /// <summary>
    /// Расчет свечей индекса сектора Нефтегаз
    /// </summary>
    /// <returns></returns>
    Task CalculateOilAndGasSectorIndexDailyCandlesAsync();
}