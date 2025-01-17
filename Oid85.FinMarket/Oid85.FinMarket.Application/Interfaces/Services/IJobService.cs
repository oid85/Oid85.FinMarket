namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис задач по расписанию
/// </summary>
public interface IJobService
{
    Task LoadInstrumentsAsync();
    Task LoadLastPricesAsync();
    Task LoadBondCouponsAsync();
    Task LoadDividendInfosAsync();
    Task LoadAssetFundamentalsAsync();
    Task LoadCandlesAsync();
    Task LoadForecastsAsync();
    Task AnalyseAsync();
    Task CalculateSpreadsAsync();
    Task CalculateMultiplicatorsAsync();
    Task CheckMarketEventsAsync();
}