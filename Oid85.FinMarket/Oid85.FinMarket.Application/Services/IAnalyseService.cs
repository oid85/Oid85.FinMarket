using Oid85.FinMarket.Domain.AnalyseResults;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services
{
    /// <summary>
    /// Сервис анализа котировок
    /// </summary>
    public interface IAnalyseService
    {
        /// <summary>
        /// Анализ с индикатором Супертренд
        /// </summary>
        public Task<List<AnalyseResult>> SupertrendAnalyseAsync(
            FinancicalInstrument stock, string timeframe);
    }
}
