namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис задач по расписанию
/// </summary>
public interface IJobService
{
    Task<bool> LoadInstrumentsAsync();
    Task<bool> LoadLastPricesAsync();
    Task<bool> LoadBondCouponsAsync();
    Task<bool> LoadDividendInfosAsync();
    Task<bool> LoadAssetFundamentalsAsync();
    Task<bool> LoadCandlesAsync();
    Task<bool> LoadForecastsAsync();
    Task<bool> AnalyseAsync();
    Task<bool> CalculateSpreadsAsync();
    Task<bool> CalculateMultiplicatorsAsync();
    Task<bool> CheckMarketEventsAsync();
    Task<bool> SendNotificationsAsync();
}