using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Results;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Services
{
    /// <inheritdoc cref="IReportService" />
    public class ReportService : ReportServiceBase, IReportService
    {
        private readonly IAnalyseResultRepository _analyseResultRepository;
        private readonly IShareRepository _shareRepository;
        private readonly IDividendInfoRepository _dividendInfoRepository;

        public ReportService(
            IAnalyseResultRepository analyseResultRepository,
            IShareRepository shareRepository,
            IDividendInfoRepository dividendInfoRepository) 
            : base(analyseResultRepository, shareRepository, dividendInfoRepository)
        {
            _analyseResultRepository = analyseResultRepository ?? throw new ArgumentNullException(nameof(analyseResultRepository));
            _shareRepository = shareRepository ?? throw new ArgumentNullException(nameof(shareRepository));
            _dividendInfoRepository = dividendInfoRepository ?? throw new ArgumentNullException(nameof(dividendInfoRepository));
        }

        /// <inheritdoc />
        public async Task<ReportData> GetReportAnalyseStock(GetReportAnalyseStockRequest request)
        {
            var reportDataSuperTrend = await GetReportDataAsync(
                request.Ticker, KnownAnalyseTypes.Supertrend, request.From, request.To);

            var reportDataCandleSequence = await GetReportDataAsync(
                request.Ticker, KnownAnalyseTypes.CandleSequence, request.From, request.To);

            var reportDataCandleVolume = await GetReportDataAsync(
                request.Ticker, KnownAnalyseTypes.CandleVolume, request.From, request.To);

            var reportDataRsi = await GetReportDataAsync(
                request.Ticker, KnownAnalyseTypes.Rsi, request.From, request.To);

            var reportData = new ReportData
            {
                Title = $"Отчет по акции {request.Ticker}",                
            };

            reportData.Header = ["Анализ"];
            reportData.Header.AddRange(reportDataSuperTrend.Header);

            List<string> reportSuperTrendData = [KnownAnalyseTypes.Supertrend];
            reportSuperTrendData.AddRange(reportDataSuperTrend.Data.First());
            reportData.Data.Add(reportSuperTrendData);

            List<string> reportCandleSequenceData = [KnownAnalyseTypes.CandleSequence];
            reportCandleSequenceData.AddRange(reportDataCandleSequence.Data.First());
            reportData.Data.Add(reportCandleSequenceData);

            List<string> reportCandleVolumeData = [KnownAnalyseTypes.CandleVolume];
            reportCandleVolumeData.AddRange(reportDataCandleVolume.Data.First());
            reportData.Data.Add(reportCandleVolumeData);

            List<string> reportRsiData = [KnownAnalyseTypes.Rsi];
            reportRsiData.AddRange(reportDataRsi.Data.First());
            reportData.Data.Add(reportRsiData);

            return reportData;
        }

        /// <inheritdoc />
        public Task<ReportData> GetReportAnalyseSupertrendStocks(
            GetReportAnalyseRequest request) =>
            GetReportDataAsync(
                request.TickerList, KnownAnalyseTypes.Supertrend, request.From, request.To);

        /// <inheritdoc />
        public Task<ReportData> GetReportAnalyseCandleSequenceStocks(
            GetReportAnalyseRequest request) =>
            GetReportDataAsync(
                request.TickerList, KnownAnalyseTypes.CandleSequence, request.From, request.To);

        /// <inheritdoc />
        public Task<ReportData> GetReportAnalyseCandleVolumeStocks(
            GetReportAnalyseRequest request) =>
            GetReportDataAsync(
                request.TickerList, KnownAnalyseTypes.CandleVolume, request.From, request.To);

        /// <inheritdoc />
        public Task<ReportData> GetReportAnalyseRsiStocks(
            GetReportAnalyseRequest request) =>
            GetReportDataAsync(
                request.TickerList, KnownAnalyseTypes.Rsi, request.From, request.To);

        /// <inheritdoc />
        public Task<ReportData> GetReportDividendsStocks() =>
            GetReportDataDividendsAsync();
    }
}
