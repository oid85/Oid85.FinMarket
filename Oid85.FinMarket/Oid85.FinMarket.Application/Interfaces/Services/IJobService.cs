namespace Oid85.FinMarket.Application.Interfaces.Services;

public interface IJobService
{
    Task LoadInstrumentsAsync();
    Task LoadPricesAsync();
    Task LoadBondCouponsAsync();
    Task LoadDividendInfosAsync();
    Task LoadDailyCandlesAsync();
    Task AnalyseAsync();
}