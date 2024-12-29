using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис отчетов
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Получить отчет по акции
    /// </summary>
    Task<ReportData> GetReportStockAnalyseAsync(GetReportAnalyseStockRequest request);

    /// <summary>
    /// Получить отчет с результатами анализа Супертренд
    /// </summary>
    Task<ReportData> GetReportStocksAnalyseSupertrendAsync(GetReportAnalyseRequest request);

    /// <summary>
    /// Получить отчет с результатами анализа Последовательность свечей одного цвета
    /// </summary>
    Task<ReportData> GetReportStocksAnalyseCandleSequenceAsync(GetReportAnalyseRequest request);

    /// <summary>
    /// Получить отчет с результатами анализа Растущий объем
    /// </summary>
    Task<ReportData> GetReportStocksAnalyseCandleVolumeAsync(GetReportAnalyseRequest request);

    /// <summary>
    /// Получить отчет с результатами анализа RSI
    /// </summary>
    Task<ReportData> GetReportStocksAnalyseRsiAsync(GetReportAnalyseRequest request);

    /// <summary>
    /// Получить отчет по доходности LTM
    /// </summary>
    Task<ReportData> ReportIndexesAnalyseYieldLtmAsync(GetReportAnalyseRequest request);
    
    /// <summary>
    /// Получить отчет по дивидендам
    /// </summary>
    Task<ReportData> GetReportDividendsAsync();

    /// <summary>
    /// Получить отчет по облигациям
    /// </summary>
    Task<ReportData> GetReportBondsAsync();

    /// <summary>
    /// Получить отчет по фундаментальным данным
    /// </summary>
    Task<ReportData> GetReportAssetFundamentalsAsync();

    /// <summary>
    /// Получить отчет по спредам
    /// </summary>
    Task<ReportData> ReportSpreadsAsync();
}