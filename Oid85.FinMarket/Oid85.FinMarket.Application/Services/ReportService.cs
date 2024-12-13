using System.Globalization;
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
        private const int WindowInDays = 180;
        
        /// <inheritdoc />
        public async Task<ReportData> GetReportAnalyseStock(GetReportAnalyseStockRequest request)
        {
            var share = await shareRepository.GetShareByTickerAsync(request.Ticker);
            
            if (share is null)
                return new ();
            
            var reportData = new ReportData();

            var dates = GetDates(request.From, request.To.AddDays(WindowInDays));
            
            reportData.Header = ["Тикер", "Сектор"];
            reportData.Header.AddRange(dates);

            reportData.Data = 
            [
                (await GetReportDataByAnalyseTypeStocks(
                    [share], request.From, request.To, KnownAnalyseTypes.Supertrend))
                .Data.First(),
                
                (await GetReportDataByAnalyseTypeStocks(
                    [share], request.From, request.To, KnownAnalyseTypes.CandleSequence))
                .Data.First(),
                
                (await GetReportDataByAnalyseTypeStocks(
                    [share], request.From, request.To, KnownAnalyseTypes.CandleVolume))
                .Data.First(),
                
                (await GetReportDataByAnalyseTypeStocks(
                    [share], request.From, request.To, KnownAnalyseTypes.Rsi))
                .Data.First(),
            ];
            
            reportData.Title =
                $"Анализ {request.Ticker} " +
                $"с {request.From.ToString(KnownDateTimeFormats.DateISO)} " +
                $"по {request.To.ToString(KnownDateTimeFormats.DateISO)}";
            
            return reportData;
        }

        /// <inheritdoc />
        public async Task<ReportData> GetReportAnalyseSupertrendStocks(GetReportAnalyseRequest request) =>
            await GetReportDataByAnalyseTypeStocks(
                await GetSharesByTickerList(request.TickerList),
                request.From,
                request.To,
                KnownAnalyseTypes.Supertrend);

        /// <inheritdoc />
        public async Task<ReportData> GetReportAnalyseCandleSequenceStocks(GetReportAnalyseRequest request) =>
            await GetReportDataByAnalyseTypeStocks(
                await GetSharesByTickerList(request.TickerList),
                request.From,
                request.To, 
                KnownAnalyseTypes.CandleSequence);

        /// <inheritdoc />
        public async Task<ReportData> GetReportAnalyseCandleVolumeStocks(GetReportAnalyseRequest request) =>
            await GetReportDataByAnalyseTypeStocks(
                await GetSharesByTickerList(request.TickerList),
                request.From,
                request.To,
                KnownAnalyseTypes.CandleVolume);

        /// <inheritdoc />
        public async Task<ReportData> GetReportAnalyseRsiStocks(GetReportAnalyseRequest request) =>
            await GetReportDataByAnalyseTypeStocks(
                await GetSharesByTickerList(request.TickerList),
                request.From,
                request.To,
                KnownAnalyseTypes.Rsi);

        /// <inheritdoc />
        public async Task<ReportData> GetReportDividendsStocks()
        {
            var dividendInfos = await dividendInfoRepository
                .GetDividendInfosAsync();
            
            var reportData = new ReportData
            {
                Title = "Информация по дивидендам",
                Header = [ "Тикер", "Фикс. р.", "Объяв.", "Размер, руб", "Дох-ть, %"]
            };
            
            foreach (var dividendInfo in dividendInfos)
            {
                reportData.Data.Add(
                [
                    dividendInfo.Ticker,
                    dividendInfo.RecordDate.ToString(KnownDateTimeFormats.DateISO),
                    dividendInfo.DeclaredDate.ToString(KnownDateTimeFormats.DateISO),
                    dividendInfo.Dividend.ToString(CultureInfo.InvariantCulture), 
                    dividendInfo.DividendPrc.ToString(CultureInfo.InvariantCulture)
                ]);
            }
            
            return reportData;
        }
        
        /// <inheritdoc />
        public async Task<ReportData> GetReportBonds()
        {
            var bonds = await bondRepository
                .GetBondsAsync();
            
            var reportData = new ReportData
            {
                Title = "Информация по облигациям",
                Header = [ "Тикер", "Сектор", "Плав. купон", "До погаш., дней"]
            };
            
            var bondCoupons = await bondCouponRepository
                .GetBondCouponsAsync(DateTime.Today, DateTime.Today.AddDays(WindowInDays));
            
            var dates = GetDates(DateTime.Today, DateTime.Today.AddDays(WindowInDays));
            
            reportData.Header.AddRange(dates);
            
            foreach (var bond in bonds)
            {
                List<string> data =
                [
                    bond.Ticker,
                    bond.Sector,
                    bond.FloatingCouponFlag ? "Да" : string.Empty,
                    (bond.MaturityDate.ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days.ToString()
                ];
                
                foreach (var date in dates)
                {
                    var bondCoupon = bondCoupons
                        .FirstOrDefault(x => 
                            x.Ticker == bond.Ticker && 
                            x.CouponDate.ToString(KnownDateTimeFormats.DateISO) == date);

                    data.Add(bondCoupon is not null 
                        ? bondCoupon.PayOneBond.ToString(CultureInfo.InvariantCulture) 
                        : string.Empty);
                }
                
                reportData.Data.Add(data);
            }
            
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
        
        private List<string> GetDates(DateTime from, DateTime to)
        {
            var curDate = from;
            var dates = new List<string>();

            while (curDate <= to)
            {
                dates.Add(curDate.ToString(KnownDateTimeFormats.DateISO));
                curDate = curDate.AddDays(1);
            }
            
            return dates;
        }
        
        private async Task<ReportData> GetReportDataByAnalyseTypeStocks(
            List<Share> shares,
            DateTime from,
            DateTime to,
            string analyseType)
        {
            var tickers = shares
                .Select(x => x.Ticker)
                .ToList();
            
            var analyseResults = (await analyseResultRepository
                .GetAnalyseResultsAsync(tickers, from, to))
                .Where(x => x.AnalyseType == analyseType)
                .ToList();

            var dividendInfos = await dividendInfoRepository
                .GetDividendInfosAsync(tickers, to.AddDays(1), to.AddDays(WindowInDays));
            
            var dates = GetDates(from, to.AddDays(WindowInDays));
            
            var reportData = new ReportData
            {
                Title = $"Анализ {analyseType} " +
                        $"с {from.ToString(KnownDateTimeFormats.DateISO)} " +
                        $"по {to.ToString(KnownDateTimeFormats.DateISO)}",
                
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

                    data.Add(analyseResult is not null 
                        ? analyseResult.Result 
                        : string.Empty);
                }
                
                foreach (var date in dates)
                {
                    var dividendInfo = dividendInfos
                        .FirstOrDefault(x => 
                            x.Ticker == share.Ticker && 
                            x.RecordDate.ToString(KnownDateTimeFormats.DateISO) == date);

                    data.Add(dividendInfo is not null
                        ? dividendInfo.DividendPrc.ToString(CultureInfo.InvariantCulture)
                        : string.Empty);
                }
                
                reportData.Data.Add(data);
            }
            
            return reportData;
        }
    }
}
