using System.Globalization;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
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
        var share = await shareRepository.GetByTickerAsync(request.Ticker);
            
        if (share is null)
            return new ();
            
        var reportData = new ReportData();

        var dates = GetDates(request.From, request.To.AddDays(WindowInDays));

        reportData.Header =
        [
            new ReportParameter(KnownDisplayTypes.String, "Тикер"),
            new ReportParameter(KnownDisplayTypes.String, "Сектор")
        ];
            
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
            await shareRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Supertrend);

    /// <inheritdoc />
    public async Task<ReportData> GetReportAnalyseCandleSequenceStocks(GetReportAnalyseRequest request) =>
        await GetReportDataByAnalyseTypeStocks(
            await shareRepository.GetWatchListAsync(),
            request.From,
            request.To, 
            KnownAnalyseTypes.CandleSequence);

    /// <inheritdoc />
    public async Task<ReportData> GetReportAnalyseCandleVolumeStocks(GetReportAnalyseRequest request) =>
        await GetReportDataByAnalyseTypeStocks(
            await shareRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.CandleVolume);

    /// <inheritdoc />
    public async Task<ReportData> GetReportAnalyseRsiStocks(GetReportAnalyseRequest request) =>
        await GetReportDataByAnalyseTypeStocks(
            await shareRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Rsi);

    /// <inheritdoc />
    public async Task<ReportData> GetReportDividendsStocks()
    {
        var dividendInfos = await dividendInfoRepository
            .GetAllAsync();
            
        var reportData = new ReportData
        {
            Title = "Информация по дивидендам",
            Header =
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Фикс. р."),
                new ReportParameter(KnownDisplayTypes.String, "Объяв."),
                new ReportParameter(KnownDisplayTypes.String, "Размер, руб"),
                new ReportParameter(KnownDisplayTypes.String, "Дох-ть, %")
            ]
        };
            
        foreach (var dividendInfo in dividendInfos)
        {
            reportData.Data.Add(
            [
                new ReportParameter(
                    KnownDisplayTypes.Ticker, 
                    dividendInfo.Ticker),
                    
                new ReportParameter(
                    KnownDisplayTypes.Date, 
                    dividendInfo.RecordDate.ToString(KnownDateTimeFormats.DateISO)),
                    
                new ReportParameter(
                    KnownDisplayTypes.Date, 
                    dividendInfo.DeclaredDate.ToString(KnownDateTimeFormats.DateISO)),
                    
                new ReportParameter(
                    KnownDisplayTypes.Ruble, 
                    dividendInfo.Dividend.ToString(CultureInfo.InvariantCulture)), 
                    
                new ReportParameter(
                    KnownDisplayTypes.Percent, 
                    dividendInfo.DividendPrc.ToString(CultureInfo.InvariantCulture))
            ]);
        }
            
        return reportData;
    }
        
    /// <inheritdoc />
    public async Task<ReportData> GetReportBonds()
    {
        var bonds = await bondRepository
            .GetWatchListAsync();
            
        var reportData = new ReportData
        {
            Title = "Информация по облигациям",
            Header =
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Сектор"),
                new ReportParameter(KnownDisplayTypes.String, "Плав. купон"),
                new ReportParameter(KnownDisplayTypes.String, "До погаш., дней"),
                new ReportParameter(KnownDisplayTypes.String, "Дох-ть., %")
            ]
        };
            
        var bondCoupons = await bondCouponRepository
            .GetAsync(DateTime.Today, DateTime.Today.AddDays(WindowInDays));
            
        var dates = GetDates(DateTime.Today, DateTime.Today.AddDays(WindowInDays));
            
        reportData.Header.AddRange(dates);
            
        foreach (var bond in bonds)
        {
            List<ReportParameter> data =
            [
                new (KnownDisplayTypes.Ticker, 
                    bond.Ticker),
                    
                new (KnownDisplayTypes.Sector, bond.Sector),
                    
                new (KnownDisplayTypes.String, 
                    bond.FloatingCouponFlag ? "Да" : string.Empty),
                    
                new (KnownDisplayTypes.String, 
                    (bond.MaturityDate.ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days.ToString())
            ];
                
            // Вычисляем полную доходность облигации
            var nextCoupon = bondCoupons
                .FirstOrDefault(x => x.Ticker == bond.Ticker);
                
            double profitPrc = 0.0;
                
            if (nextCoupon is not null)
                profitPrc = (bond.Price / (365.0 / nextCoupon.CouponPeriod) * nextCoupon.PayOneBond) / 100.0;
                
            data.Add(new ReportParameter(
                KnownDisplayTypes.Percent, 
                profitPrc.ToString(CultureInfo.InvariantCulture)));
                
            foreach (var date in dates)
            {
                var bondCoupon = bondCoupons
                    .FirstOrDefault(x => 
                        x.Ticker == bond.Ticker &&
                        x.CouponDate.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(bondCoupon is not null 
                    ? new ReportParameter(
                        KnownDisplayTypes.Ruble, 
                        bondCoupon.PayOneBond.ToString(CultureInfo.InvariantCulture)) 
                    : new ReportParameter(
                        KnownDisplayTypes.Ruble, 
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }

    public async Task<ReportData> GetReportAssetFundamentalsStocks()
    {
        var reportData = new ReportData();
        return reportData;
    }

    private List<ReportParameter> GetDates(DateTime from, DateTime to)
    {
        var curDate = from;
        var dates = new List<ReportParameter>();

        while (curDate <= to)
        {
            dates.Add(new ReportParameter(
                KnownDisplayTypes.Date,
                curDate.ToString(KnownDateTimeFormats.DateISO)));
                
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
        var instrumentIds = shares
            .Select(x => x.InstrumentId)
            .ToList();        
        
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => x.AnalyseType == analyseType)
            .ToList();

        var dividendInfos = await dividendInfoRepository
            .GetAsync(instrumentIds, to.AddDays(1), to.AddDays(WindowInDays));
            
        var dates = GetDates(from, to.AddDays(WindowInDays));
            
        var reportData = new ReportData
        {
            Title = $"Анализ {analyseType} " +
                    $"с {from.ToString(KnownDateTimeFormats.DateISO)} " +
                    $"по {to.ToString(KnownDateTimeFormats.DateISO)}",
                
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Сектор")
            ]
        };

        reportData.Header.AddRange(dates);

        foreach (var share in shares)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, share.Ticker), 
                new (KnownDisplayTypes.Sector, share.Sector)
            };

            foreach (var date in dates)
            {
                var analyseResult = analyseResults
                    .FirstOrDefault(x => 
                        x.InstrumentId == share.InstrumentId && 
                        x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(analyseResult is not null 
                    ? new ReportParameter(
                        KnownDisplayTypes.AnalyseTypeValue, 
                        analyseResult.Result) 
                    : new ReportParameter(
                        KnownDisplayTypes.AnalyseTypeValue, 
                        string.Empty));
            }
                
            foreach (var date in dates)
            {
                var dividendInfo = dividendInfos
                    .FirstOrDefault(x => 
                        x.Ticker == share.Ticker && 
                        x.RecordDate.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(dividendInfo is not null 
                    ? new ReportParameter(
                        KnownDisplayTypes.Percent, 
                        dividendInfo.DividendPrc.ToString(CultureInfo.InvariantCulture)) 
                    : new ReportParameter(
                        KnownDisplayTypes.Percent, 
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
}