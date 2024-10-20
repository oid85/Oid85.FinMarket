using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Results;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Storage;
using System.Collections.Generic;

namespace Oid85.FinMarket.Application.Services
{
    /// <inheritdoc />
    public class ReportService : ReportServiceBase, IReportService
    {
        private readonly IStorageService _storageService;
        private readonly ICatalogService _catalogService;

        public ReportService(
            IStorageService storageService,
            ICatalogService catalogService) 
            : base(storageService, catalogService)
        {
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        }

        /// <inheritdoc />
        public async Task<ReportData> GetReportAnalyseStock(GetReportAnalyseStockRequest request)
        {
            var reportDataSuperTrend = await GetReportDataAsync(
                request.Ticker, KnownAnalyseTypes.Supertrend, request.From, request.To);

            var reportDataCandleSequence = await GetReportDataAsync(
                request.Ticker, KnownAnalyseTypes.CandleSequence, request.From, request.To);

            var reportData = new ReportData
            {
                Title = "Отчет по акции",
                
            };

            reportData.Header = ["Анализ"];
            reportData.Header.AddRange(reportDataSuperTrend.Header);

            List<string> reportDataSuperTrendData = [KnownAnalyseTypes.Supertrend];
            reportDataSuperTrendData.AddRange(reportDataSuperTrend.Data.First());
            reportData.Data.Add(reportDataSuperTrendData);

            List<string> reportDataCandleSequenceData = [KnownAnalyseTypes.CandleSequence];
            reportDataCandleSequenceData.AddRange(reportDataCandleSequence.Data.First());
            reportData.Data.Add(reportDataCandleSequenceData);

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
        public Task<ReportData> GetReportAnalyseVolumeStocks(GetReportAnalyseRequest request) =>
            GetReportDataAsync(
                request.TickerList, KnownAnalyseTypes.Volume, request.From, request.To);

    }
}
