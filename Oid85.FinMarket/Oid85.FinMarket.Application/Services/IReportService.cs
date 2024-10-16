using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Results;

namespace Oid85.FinMarket.Application.Services
{
    /// <summary>
    /// Сервис отчетов
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Получить отчет с результатами анализа Супертренд
        /// </summary>
        Task<ReporData> GetReportAnalyseSupertrendStocks(
            GetReportAnalyseRequest request);

        /// <summary>
        /// Получить отчет с результатами анализа Последовательность свечей одного цвета
        /// </summary>
        Task<ReporData> GetReportAnalyseCandleSequenceStocks(
            GetReportAnalyseRequest request);
    }
}
