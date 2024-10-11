using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Results;

namespace Oid85.FinMarket.Application.Services
{
    /// <summary>
    /// Сервис отчетов
    /// </summary>
    public interface IReportService
    {
        Task<ReporData> GetReportAnalyseSupertrendStocks(
            GetReportAnalyseSupertrendRequest request);
    }
}
