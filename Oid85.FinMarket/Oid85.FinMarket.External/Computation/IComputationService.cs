namespace Oid85.FinMarket.External.Computation;

/// <summary>
/// Работа с сервисом расчетов
/// </summary>
public interface IComputationService
{
    /// <summary>
    /// Выполнить проверку рядов на стационарность
    /// </summary>
    /// <returns></returns>
    Task<List<bool>> CheckStationaryAsync(List<List<double>> data);
}