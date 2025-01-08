using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Interfaces.Services.ReportServices;

/// <summary>
/// Сервис отчетов по акциям
/// </summary>
public interface ISharesReportService
{
    /// <summary>
    /// Отчет Аггрегированный анализ
    /// </summary>
    Task<ReportData> GetAggregatedAnalyseAsync(
        GetAnalyseRequest request);

    /// <summary>
    /// Отчет Анализ Супертренд
    /// </summary>
    Task<ReportData> GetSupertrendAnalyseAsync(
        GetAnalyseRequest request);

    /// <summary>
    /// Отчет Анализ Последовательность свечей одного цвета
    /// </summary>
    Task<ReportData> GetCandleSequenceAnalyseAsync(
        GetAnalyseRequest request);

    /// <summary>
    /// Отчет Анализ Растущий объем
    /// </summary>
    Task<ReportData> GetCandleVolumeAnalyseAsync(
        GetAnalyseRequest request);

    /// <summary>
    /// Отчет Анализ RSI
    /// </summary>
    Task<ReportData> GetRsiAnalyseAsync(
        GetAnalyseRequest request);
    
    /// <summary>
    /// Отчет Доходность LTM
    /// </summary>
    Task<ReportData> GetYieldLtmAnalyseAsync(
        GetAnalyseRequest request);
    
    /// <summary>
    /// Отчет Дивиденды
    /// </summary>
    Task<ReportData> GetDividendAnalyseAsync();
    
    /// <summary>
    /// Отчет Фундаментальные данные
    /// </summary>
    Task<ReportData> GetAssetFundamentalAnalyseAsync();
}