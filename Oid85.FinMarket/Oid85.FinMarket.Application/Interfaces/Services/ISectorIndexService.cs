namespace Oid85.FinMarket.Application.Interfaces.Services;

public interface ISectorIndexService
{
    /// <summary>
    /// Расчет виртуальных свечей сектора Нефтегаз
    /// </summary>
    /// <returns></returns>
    Task CalculateOilAndGasSectorIndexDailyCandlesAsync();
}