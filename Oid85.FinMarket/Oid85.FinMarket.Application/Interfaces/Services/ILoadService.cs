﻿namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис загрузки данных
/// </summary>
public interface ILoadService
{
    Task LoadSharesAsync();
    Task LoadShareLastPricesAsync();
    Task LoadShareDailyCandlesAsync();
    Task LoadShareHourlyCandlesAsync();
    Task<bool> LoadHistoryShareDailyCandlesAsync();
    Task<bool> LoadHistoryShareHourlyCandlesAsync();
    Task LoadForecastsAsync();
    Task LoadAssetReportEventsAsync();
    
    Task LoadFuturesAsync();
    Task LoadFutureLastPricesAsync();
    Task LoadFutureDailyCandlesAsync();
    
    Task LoadBondsAsync();
    Task LoadBondCouponsAsync();
    Task LoadBondLastPricesAsync();
    Task LoadBondDailyCandlesAsync();
    
    Task LoadDividendInfosAsync();
    
    Task LoadIndexesAsync();
    Task LoadIndexLastPricesAsync();
    Task LoadIndexDailyCandlesAsync();
    
    Task LoadCurrenciesAsync();
    Task LoadCurrencyLastPricesAsync();
    Task LoadCurrencyDailyCandlesAsync();
}