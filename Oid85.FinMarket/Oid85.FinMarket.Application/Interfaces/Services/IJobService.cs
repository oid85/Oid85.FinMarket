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
    Task LoadDailyCandlesAsync();
    Task LoadFiveMinuteCandlesAsync();
    Task AnalyseAsync();
    Task FillingSpreadPairsAsync();
    Task CalculateSpreadsAsync();
}