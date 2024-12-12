using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services
{
    /// <inheritdoc cref="IReportService" />
    public class ReportService(
        IAnalyseResultRepository analyseResultRepository,
        IBondCouponRepository bondCouponRepository,
        IBondRepository bondRepository,
        IDividendInfoRepository dividendInfoRepository,
        IShareRepository shareRepository)
        : IReportService
    {
        /// <inheritdoc />
        public async Task<ReportData> GetReportAnalyseStock(GetReportAnalyseStockRequest request)
        {
            var share = shareRepository.GetShareByTickerAsync(request.Ticker);
            
            var reportData = new ReportData();
            return reportData;
        }

        /// <inheritdoc />
        public async Task<ReportData> GetReportAnalyseSupertrendStocks(GetReportAnalyseRequest request)
        {
            return await GetReportDataByAnalyseTypeStocks(request, KnownAnalyseTypes.Supertrend);
        }

        /// <inheritdoc />
        public async Task<ReportData> GetReportAnalyseCandleSequenceStocks(GetReportAnalyseRequest request)
        {
            var shares = GetSharesByTickerList(request.TickerList);
            
            var reportData = new ReportData();
            return reportData;
        }

        /// <inheritdoc />
        public async Task<ReportData> GetReportAnalyseCandleVolumeStocks(GetReportAnalyseRequest request)
        {
            var shares = GetSharesByTickerList(request.TickerList);
            
            var reportData = new ReportData();
            return reportData;
        }

        /// <inheritdoc />
        public async Task<ReportData> GetReportAnalyseRsiStocks(GetReportAnalyseRequest request)
        {
            var shares = GetSharesByTickerList(request.TickerList);
            
            var reportData = new ReportData();
            return reportData;
        }

        /// <inheritdoc />
        public async Task<ReportData> GetReportDividendsStocks()
        {
            var reportData = new ReportData();
            return reportData;
        }
        
        /// <inheritdoc />
        public async Task<ReportData> GetReportBonds()
        {
            var reportData = new ReportData();
            return reportData;
        }

        private async Task<List<Share>> GetSharesByTickerList(string tickerList)
        {
            if (tickerList == KnownTickerLists.AllStocks)
                return await shareRepository.GetSharesAsync();
            
            if (tickerList == KnownTickerLists.MoexIndexStocks)
                return await shareRepository.GetMoexIndexSharesAsync();
            
            if (tickerList == KnownTickerLists.PortfolioStocks)
                return await shareRepository.GetPortfolioSharesAsync();
            
            if (tickerList == KnownTickerLists.WatchListStocks)
                return await shareRepository.GetWatchListSharesAsync();

            return [];
        }
        
        private List<string> GetDates(DateTime from, DateTime to, int addDays)
        {
            var endDate = to.AddDays(addDays);
            var curDate = from;
            
            var dates = new List<string>();

            while (curDate <= endDate)
            {
                dates.Add(curDate.ToString(KnownDateTimeFormats.DateISO));
                curDate = curDate.AddDays(1);
            }
            
            return dates;
        }
        
        private async Task<ReportData> GetReportDataByAnalyseTypeStocks(
            GetReportAnalyseRequest request,
            string analyseType)
        {
            var shares = await GetSharesByTickerList(request.TickerList);
            
            var tickers = shares
                .Select(x => x.Ticker)
                .ToList();
            
            var analyseResults = (await analyseResultRepository
                .GetAnalyseResultsAsync(tickers, request.From, request.To))
                .Where(x => x.AnalyseType == analyseType)
                .ToList();

            const int addDays = 90;
            
            var dividendInfos = await dividendInfoRepository
                .GetDividendInfosAsync(tickers, request.To.AddDays(1), request.To.AddDays(addDays));
            
            var dates = GetDates(request.From, request.To, addDays);
            
            var reportData = new ReportData
            {
                Title = $"Анализ Супертренд {request.From.ToString(KnownDateTimeFormats.DateISO)} с по {request.To.ToString(KnownDateTimeFormats.DateISO)}",
                Header = ["Тикер", "Сектор"]
            };

            reportData.Header.AddRange(dates);

            foreach (var share in shares)
            {
                var data = new List<string> { share.Ticker, share.Sector };

                foreach (var date in dates)
                {
                    var analyseResult = analyseResults
                        .FirstOrDefault(x => 
                            x.Ticker == share.Ticker && 
                            x.Date.ToString(KnownDateTimeFormats.DateISO) == date);
                    
                    if (analyseResult is not null)
                        data.Add(analyseResult.Result);
                }
                
                foreach (var date in dates)
                {
                    var dividendInfo = dividendInfos
                        .FirstOrDefault(x => 
                            x.Ticker == share.Ticker && 
                            x.RecordDate.ToString(KnownDateTimeFormats.DateISO) == date);
                    
                    if (dividendInfo is not null)
                        data.Add(dividendInfo.DividendPrc.ToString("N1"));
                }
                
                reportData.Data.Add(data);
            }
            
            return reportData;
        }
    }
}
