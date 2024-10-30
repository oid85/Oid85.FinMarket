using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Services
{
    /// <summary>
    /// Сервис анализа
    /// </summary>
    public interface IAnalyseService
    {
        /// <summary>
        /// Анализ всех акций
        /// </summary>
        public Task AnalyseStocksAsync();

        /// <summary>
        /// Анализ с индикатором Супертренд
        /// </summary>
        public Task<List<AnalyseResult>> SupertrendAnalyseAsync(
            Share stock, string timeframe);

        /// <summary>
        /// Анализ последовательности подряд идущих свечей
        /// </summary>
        public Task<List<AnalyseResult>> CandleSequenceAnalyseAsync(
            Share stock, string timeframe);

        /// <summary>
        /// Анализ растущего объема
        /// </summary>
        public Task<List<AnalyseResult>> CandleVolumeAnalyseAsync(
            Share stock, string timeframe);

        /// <summary>
        /// Анализ RSI
        /// </summary>
        public Task<List<AnalyseResult>> RsiAnalyseAsync(
            Share stock, string timeframe);
    }
}
