﻿using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Results;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Storage;

namespace Oid85.FinMarket.Application.Services
{
    /// <inheritdoc />
    public class ReportService : IReportService
    {
        private readonly IStorageService _storageService;
        private readonly ICatalogService _catalogService;

        public ReportService(
            IStorageService storageService,
            ICatalogService catalogService)
        {
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        }

        /// <inheritdoc />
        public async Task<ReporData> GetReportAnalyseSupertrendStocks(
            GetReportAnalyseRequest request)
        {
            if (request.TickerList == KnownTickerLists.AllStocks)
            {
                var tickers = (await _catalogService
                    .GetActiveFinInstrumentsAsync(KnownFinInstrumentTypes.Stocks))
                    .OrderBy(x => x.Sector)
                    .Select(x => x.Ticker);

                var data = await GetDataAsync(KnownAnalyseTypes.Supertrend, tickers, request.From, request.To);

                var reportData = await GetReportDataByTickerListAsync(KnownAnalyseTypes.Supertrend, data, tickers, request.TickerList);

                return reportData;
            }

            if (request.TickerList == KnownTickerLists.MoexIndexStocks)
            {                
                var tickers = (await _catalogService.GetMoexIndexItemsAsync()).Select(x => x.Ticker);
                
                var data = await GetDataAsync(KnownAnalyseTypes.Supertrend, tickers, request.From, request.To);

                var reportData = await GetReportDataByTickerListAsync(KnownAnalyseTypes.Supertrend, data, tickers, request.TickerList);

                return reportData;
            }

            if (request.TickerList == KnownTickerLists.PortfolioStocks)
            {
                var tickers = (await _catalogService.GetPortfolioItemsAsync()).Select(x => x.Ticker);

                var data = await GetDataAsync(KnownAnalyseTypes.Supertrend, tickers, request.From, request.To);

                var reportData = await GetReportDataByTickerListAsync(KnownAnalyseTypes.Supertrend, data, tickers, request.TickerList);

                return reportData;
            }

            if (request.TickerList == KnownTickerLists.WatchListStocks)
            {
                var tickers = (await _catalogService.GetWatchListItemsAsync()).Select(x => x.Ticker);

                var data = await GetDataAsync(KnownAnalyseTypes.Supertrend, tickers, request.From, request.To);

                var reportData = await GetReportDataByTickerListAsync(KnownAnalyseTypes.Supertrend, data, tickers, request.TickerList);

                return reportData;
            }

            return new();
        }

        private async Task<ReporData> GetReportDataByTickerListAsync(
            string analyseType, 
            Dictionary<string, List<Tuple<string, string>>> data, 
            IEnumerable<string> tickers,
            string tickerList)
        {
            var reportData = new ReporData()
            {
                Title = $"{analyseType} {tickerList}"
            };

            reportData.Header = ["Тикер", "Сектор"];

            var dates = data.Keys
                .Select(x => x.ToString())
                .OrderBy(x => Convert.ToDateTime(x))
                .ToList();

            reportData.Header.AddRange(dates);

            foreach (var ticker in tickers)
            {
                string sector = (await _catalogService.GetFinInstrumentAsync(
                    KnownFinInstrumentTypes.Stocks, ticker))!.Sector;

                var tickerData = new List<string>() { ticker, sector };

                foreach (var date in dates)
                {
                    var item = data[date].FirstOrDefault(x => x.Item1 == ticker);

                    if (item is null)
                        tickerData.Add(string.Empty);
                    else
                        tickerData.Add(item.Item2);
                }

                reportData.Data.Add(tickerData);
            }

            return reportData;
        }

        private async Task<Dictionary<string, List<Tuple<string, string>>>> GetDataAsync(
            string analyseType,
            IEnumerable<string> tickers,
            DateTime from,
            DateTime to)
        {
            var data = new Dictionary<string, List<Tuple<string, string>>>();

            foreach (var ticker in tickers)
            {
                var analyseResults = await _storageService.GetAnalyseResultsAsync(
                    $"{ticker.ToLower()}_{analyseType.Replace(" ", "_").ToLower()}_daily", from, to);

                foreach (var analyseResult in analyseResults)
                {
                    string key = analyseResult.Date.ToShortDateString();

                    if (!data.ContainsKey(key))
                        data[key] = [new(analyseResult.Ticker, analyseResult.TrendDirection)];

                    else
                        data[key].Add(new(analyseResult.Ticker, analyseResult.TrendDirection));
                }
            }

            return data;
        }
    }
}
