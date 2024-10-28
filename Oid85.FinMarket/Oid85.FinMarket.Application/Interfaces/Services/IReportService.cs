using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Results;

namespace Oid85.FinMarket.Application.Interfaces.Services
{
    /// <summary>
    /// Сервис отчетов
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Получить отчет по акции
        /// </summary>
        Task<ReportData> GetReportAnalyseStock(GetReportAnalyseStockRequest request);

        /// <summary>
        /// Получить отчет с результатами анализа Супертренд
        /// </summary>
        Task<ReportData> GetReportAnalyseSupertrendStocks(GetReportAnalyseRequest request);

        /// <summary>
        /// Получить отчет с результатами анализа Последовательность свечей одного цвета
        /// </summary>
        Task<ReportData> GetReportAnalyseCandleSequenceStocks(GetReportAnalyseRequest request);

        /// <summary>
        /// Получить отчет с результатами анализа Растущий объем
        /// </summary>
        Task<ReportData> GetReportAnalyseCandleVolumeStocks(GetReportAnalyseRequest request);

        /// <summary>
        /// Получить отчет с результатами анализа RSI
        /// </summary>
        Task<ReportData> GetReportAnalyseRsiStocks(GetReportAnalyseRequest request);

        /// <summary>
        /// Получить отчет по дивидендам
        /// </summary>
        Task<ReportData> GetReportDividendsStocks();
    }
}
