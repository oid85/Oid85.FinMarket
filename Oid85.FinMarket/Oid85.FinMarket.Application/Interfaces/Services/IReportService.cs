using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис отчетов
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Отчет Анализ акции
    /// </summary>
    Task<ReportData> GetReportShareAnalyseAsync(GetReportAnalyseByTickerRequest request);

    /// <summary>
    /// Отчет Анализ Супертренд
    /// </summary>
    Task<ReportData> GetReportSharesAnalyseSupertrendAsync(GetReportAnalyseRequest request);

    /// <summary>
    /// Отчет Анализ Последовательность свечей одного цвета
    /// </summary>
    Task<ReportData> GetReportSharesAnalyseCandleSequenceAsync(GetReportAnalyseRequest request);

    /// <summary>
    /// Отчет Анализ Растущий объем
    /// </summary>
    Task<ReportData> GetReportSharesAnalyseCandleVolumeAsync(GetReportAnalyseRequest request);

    /// <summary>
    /// Отчет Анализ RSI
    /// </summary>
    Task<ReportData> GetReportSharesAnalyseRsiAsync(GetReportAnalyseRequest request);

    /// <summary>
    /// Отчет Доходность LTM
    /// </summary>
    Task<ReportData> GetReportIndexesAnalyseYieldLtmAsync(GetReportAnalyseRequest request);
    
    /// <summary>
    /// Отчет Дивиденды
    /// </summary>
    Task<ReportData> GetReportDividendsAsync();

    /// <summary>
    /// Отчет Облигации
    /// </summary>
    Task<ReportData> GetReportBondsAsync();

    /// <summary>
    /// Отчет Анализ Супертренд
    /// </summary>
    Task<ReportData> GetReportBondsAnalyseSupertrendAsync(GetReportAnalyseRequest request);
    
    /// <summary>
    /// Отчет Анализ Последовательность свечей одного цвета
    /// </summary>
    Task<ReportData> GetReportBondsAnalyseCandleSequenceAsync(GetReportAnalyseRequest request);
    
    /// <summary>
    /// Отчет Фундаментальные данные
    /// </summary>
    Task<ReportData> GetReportAssetFundamentalsAsync();

    /// <summary>
    /// Отчет Спреды
    /// </summary>
    Task<ReportData> GetReportSpreadsAsync();

    /// <summary>
    /// Отчет Анализ Супертренд
    /// </summary>
    Task<ReportData> GetReportCurrenciesAnalyseSupertrendAsync(GetReportAnalyseRequest request);
    
    /// <summary>
    /// Отчет Анализ Последовательность свечей одного цвета
    /// </summary>
    Task<ReportData> GetReportCurrenciesAnalyseCandleSequenceAsync(GetReportAnalyseRequest request);
    
    /// <summary>
    /// Отчет Анализ RSI
    /// </summary>
    Task<ReportData> GetReportFuturesAnalyseRsiAsync(GetReportAnalyseRequest request);
    
    /// <summary>
    /// Отчет Анализ Растущий объем
    /// </summary>
    Task<ReportData> GetReportFuturesAnalyseCandleVolumeAsync(GetReportAnalyseRequest request);
    
    /// <summary>
    /// Отчет Анализ Последовательность свечей одного цвета
    /// </summary>
    Task<ReportData> GetReportFuturesAnalyseCandleSequenceAsync(GetReportAnalyseRequest request);
    
    /// <summary>
    /// Отчет Анализ Супертренд
    /// </summary>
    Task<ReportData> GetReportFuturesAnalyseSupertrendAsync(GetReportAnalyseRequest request);
    
    /// <summary>
    /// Отчет Анализ Супертренд
    /// </summary>
    Task<ReportData> GetReportIndexesAnalyseSupertrendAsync(GetReportAnalyseRequest request);
    
    /// <summary>
    /// Отчет Анализ Последовательность свечей одного цвета
    /// </summary>
    Task<ReportData> GetReportIndexesAnalyseCandleSequenceAsync(GetReportAnalyseRequest request);
}