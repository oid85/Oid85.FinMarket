using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Application.Models.Results;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Storage;
using static Google.Rpc.Context.AttributeContext.Types;

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
            GetReportAnalyseSupertrendRequest request)
        {
            if (request.TickerList == KnownTickerLists.MoexIndexStocks)
            {                
                var tickers = (await _catalogService.GetMoexIndexItemsAsync()).Select(x => x.Ticker);
                
                var data = await GetDataAsync(tickers, request.From, request.To);

                var reportData = GetReportData(
                    data, 
                    tickers, 
                    request.TickerList);

                return reportData;
            }

            if (request.TickerList == KnownTickerLists.PortfolioStocks)
            {
                var tickers = (await _catalogService.GetPortfolioItemsAsync()).Select(x => x.Ticker);

                var data = await GetDataAsync(tickers, request.From, request.To);

                var reportData = GetReportData(
                    data,
                    tickers,
                    request.TickerList);

                return reportData;
            }

            if (request.TickerList == KnownTickerLists.WatchListStocks)
            {
                var tickers = (await _catalogService.GetWatchListItemsAsync()).Select(x => x.Ticker);

                var data = await GetDataAsync(tickers, request.From, request.To);

                var reportData = GetReportData(
                    data,
                    tickers,
                    request.TickerList);

                return reportData;
            }

            return new();
        }

        private static ReporData GetReportData(
            Dictionary<string, List<Tuple<string, string>>> data, 
            IEnumerable<string> tickers,
            string tickerList)
        {
            var reportData = new ReporData()
            {
                Title = $"Report Analyse Supertrend {tickerList}"
            };

            reportData.Header = ["Тикер"];

            var dates = data.Keys
                .Select(x => x.ToString())
                .OrderBy(x => Convert.ToDateTime(x))
                .ToList();

            reportData.Header.AddRange(dates);

            foreach (var ticker in tickers)
            {
                var tickerData = new List<string>() { ticker };

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
            IEnumerable<string> tickers,
            DateTime from,
            DateTime to)
        {
            var data = new Dictionary<string, List<Tuple<string, string>>>();

            foreach (var ticker in tickers)
            {
                var analyseResults = await _storageService.GetAnalyseResultsAsync(
                    $"{ticker.ToLower()}_supertrend_daily", from, to);

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
