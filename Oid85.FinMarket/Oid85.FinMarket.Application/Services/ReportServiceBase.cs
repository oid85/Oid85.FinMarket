﻿using Oid85.FinMarket.Application.Models.Results;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Storage;

namespace Oid85.FinMarket.Application.Services
{
    /// <inheritdoc />
    public class ReportServiceBase
    {
        private readonly IStorageService _storageService;
        private readonly ICatalogService _catalogService;

        public ReportServiceBase(
            IStorageService storageService,
            ICatalogService catalogService)
        {
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        }

        public async Task<ReportData> GetReportDataAsync(
            string tickerList, 
            string analyseType,
            DateTime from,
            DateTime to)
        {
            if (tickerList == KnownTickerLists.AllStocks)
            {
                var tickers = (await _catalogService
                    .GetActiveFinInstrumentsAsync(KnownFinInstrumentTypes.Stocks))
                .Select(x => x.Ticker);

                var data = await GetDataAsync(analyseType, tickers, from, to);
                
                var reportData = await GetReportDataByTickerListAsync(
                    analyseType, data, tickers);

                reportData.Title = $"{analyseType} {tickerList}";                

                return reportData;
            }

            if (tickerList == KnownTickerLists.MoexIndexStocks)
            {
                var tickers = (await _catalogService.GetMoexIndexStocksAsync())
                    .Select(x => x.Ticker);
                
                var data = await GetDataAsync(analyseType, tickers, from, to);

                var reportData = await GetReportDataByTickerListAsync(
                    analyseType, data, tickers);

                reportData.Title = $"{analyseType} {tickerList}";

                return reportData;
            }

            if (tickerList == KnownTickerLists.PortfolioStocks)
            {
                var tickers = (await _catalogService.GetPortfolioStocksAsync())
                    .Select(x => x.Ticker);
                
                var data = await GetDataAsync(analyseType, tickers, from, to);

                var reportData = await GetReportDataByTickerListAsync(
                    analyseType, data, tickers);

                reportData.Title = $"{analyseType} {tickerList}";

                return reportData;
            }

            if (tickerList == KnownTickerLists.WatchListStocks)
            {
                var tickers = (await _catalogService.GetWatchListStocksAsync())
                    .Select(x => x.Ticker);
                
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

            var dividendInfos = await _catalogService.GetDividendInfosAsync();

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

            var divinendInfos = await _catalogService.GetDividendInfosAsync();
            
            foreach (var ticker in tickers)
            {
                var analyseResults = await _storageService.GetAnalyseResultsAsync(
                    $"{ticker.ToLower()}_{analyseType.Replace(" ", "_").ToLower()}_daily", from, to);

                if (analyseResults is [])
                    continue;

                var lastDate = analyseResults.Last().Date;
                var timeframe = analyseResults.Last().Timeframe;

                var divinendInfosByTicker = divinendInfos
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
                        Data = string.Empty,
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
