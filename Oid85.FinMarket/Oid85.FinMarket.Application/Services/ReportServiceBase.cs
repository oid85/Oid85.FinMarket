using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Models.Results;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services
{
    /// <inheritdoc />
    public class ReportServiceBase
    {
        private readonly IAnalyseResultRepository _analyseResultRepository;
        private readonly IShareRepository _shareRepository;
        private readonly IDividendInfoRepository _dividendInfoRepository;

        public ReportServiceBase(
            IAnalyseResultRepository analyseResultRepository,
            IShareRepository shareRepository,
            IDividendInfoRepository dividendInfoRepository) 
        {
            _analyseResultRepository = analyseResultRepository ?? throw new ArgumentNullException(nameof(analyseResultRepository));
            _shareRepository = shareRepository ?? throw new ArgumentNullException(nameof(shareRepository));
            _dividendInfoRepository = dividendInfoRepository ?? throw new ArgumentNullException(nameof(dividendInfoRepository));
        }

        public async Task<ReportData> GetReportDataAsync(
            string tickerList, 
            string analyseType,
            DateTime from,
            DateTime to)
        {
            if (tickerList == KnownTickerLists.AllStocks)
            {
                var shares = await _shareRepository.GetSharesAsync();
                var tickers = shares.Select(x => x.Ticker);

                var data = await GetDataAsync(analyseType, tickers, from, to);
                
                var reportData = await GetReportDataByTickerListAsync(
                    analyseType, data, tickers);

                reportData.Title = $"{analyseType} {tickerList}";                

                return reportData;
            }

            if (tickerList == KnownTickerLists.MoexIndexStocks)
            {
                var shares = await _shareRepository.GetMoexIndexSharesAsync();
                var tickers = shares.Select(x => x.Ticker);
                
                var data = await GetDataAsync(analyseType, tickers, from, to);

                var reportData = await GetReportDataByTickerListAsync(
                    analyseType, data, tickers);

                reportData.Title = $"{analyseType} {tickerList}";

                return reportData;
            }

            if (tickerList == KnownTickerLists.PortfolioStocks)
            {
                var shares = await _shareRepository.GetPortfolioSharesAsync();
                var tickers = shares.Select(x => x.Ticker);
                
                var data = await GetDataAsync(analyseType, tickers, from, to);

                var reportData = await GetReportDataByTickerListAsync(
                    analyseType, data, tickers);

                reportData.Title = $"{analyseType} {tickerList}";

                return reportData;
            }

            if (tickerList == KnownTickerLists.WatchListStocks)
            {
                var shares = await _shareRepository.GetWatchListSharesAsync();
                var tickers = shares.Select(x => x.Ticker);
                
                var data = await GetDataAsync(analyseType, tickers, from, to);

                var reportData = await GetReportDataByTickerListAsync(
                    analyseType, data, tickers);

                reportData.Title = $"{analyseType} {tickerList}";

                return reportData;
            }

            else
            {
                var tickers = new List<string> { tickerList };

                var data = await GetDataAsync(analyseType, tickers, from, to);

                var reportData = await GetReportDataByTickerListAsync(
                    analyseType, data, tickers);

                reportData.Title = $"{analyseType} {tickerList}";

                return reportData;
            }            
        }

        public async Task<ReportData> GetReportDataDividendsAsync()
        {
            var reportData = new ReportData() 
            { 
                Title = "Dividends",
                Header = [ "Тикер", "Дата фикс. реестра", "Дата объяв.", "Размер, руб", "Доходность, %"]
            };

            var dividendInfos = await _dividendInfoRepository.GetDividendInfosAsync();

            foreach (var dividendInfo in dividendInfos)
            {
                reportData.Data.Add(
                    [
                        dividendInfo.Ticker,
                        dividendInfo.RecordDate.ToString(KnownDateTimeFormats.DateISO),
                        dividendInfo.DeclaredDate.ToString(KnownDateTimeFormats.DateISO),
                        dividendInfo.Dividend.ToString(), 
                        dividendInfo.DividendPrc.ToString()
                    ]);
            }

            return reportData;
        }

        private async Task<ReportData> GetReportDataByTickerListAsync(
            string analyseType, 
            Dictionary<string, List<Tuple<string, string>>> data, 
            IEnumerable<string> tickers)
        {
            var reportData = new ReportData
            {
                Header = ["Тикер", "Сектор"]
            };

            var dates = data.Keys
                .Select(x => x.ToString())
                .OrderBy(x => Convert.ToDateTime(x))
                .ToList();

            reportData.Header.AddRange(dates);

            foreach (var ticker in tickers)
            {
                string sector = (await _shareRepository.GetShareByTickerAsync(ticker))!.Sector;

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

            var dividendInfos = await _dividendInfoRepository.GetDividendInfosAsync();
            
            foreach (var ticker in tickers)
            {
                var analyseResults = await _analyseResultRepository.GetAnalyseResultsAsync(ticker, from, to);

                if (analyseResults is [])
                    continue;

                var lastDate = analyseResults.Last().Date;
                var timeframe = analyseResults.Last().Timeframe;

                var divinendInfosByTicker = dividendInfos
                    .Where(x => x.Ticker == ticker)
                    .ToList();

                // Добавляем окно для дивидендных событий
                for (int i = 0; i < 180; i++)
                {
                    lastDate = lastDate.AddDays(1);

                    string result = string.Empty;

                    if (divinendInfosByTicker.Any())
                    {
                        var divinendInfo = divinendInfosByTicker.FirstOrDefault(
                            x => x.RecordDate.ToString(KnownDateTimeFormats.DateISO) ==
                            lastDate.ToString(KnownDateTimeFormats.DateISO));

                        if (divinendInfo is not null)
                            result = $"Dividend: {divinendInfo.DividendPrc.ToString("N0")}";
                    }

                    var analyseResult = new AnalyseResult
                    {
                        Ticker = ticker,
                        Date = lastDate,
                        Timeframe = timeframe,
                        Result = result
                    };

                    analyseResults.Add(analyseResult);
                }

                foreach (var analyseResult in analyseResults)
                {
                    string key = analyseResult.Date.ToString(KnownDateTimeFormats.DateISO);

                    if (!data.ContainsKey(key))
                        data[key] = [new(analyseResult.Ticker, analyseResult.Result)];

                    else
                        data[key].Add(new(analyseResult.Ticker, analyseResult.Result));
                }                
            }

            return data;
        }
    }
}
