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
    ICurrencyRepository currencyRepository,
    IDividendInfoRepository dividendInfoRepository,
    IIndexRepository indexRepository,
    IShareRepository shareRepository,
    IFutureRepository futureRepository,
    IAssetFundamentalRepository assetFundamentalRepository,
    ISpreadRepository spreadRepository)
    : IReportService
{
    private const int WindowInDays = 180;
        


    #region Bonds
    
    /// <inheritdoc />
    public async Task<ReportData> GetReportBondsAsync()
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
            .GetAsync(
                DateOnly.FromDateTime(DateTime.Today), 
                DateOnly.FromDateTime(DateTime.Today).AddDays(WindowInDays));
            
        var dates = GetDates(
            DateOnly.FromDateTime(DateTime.Today), 
            DateOnly.FromDateTime(DateTime.Today).AddDays(WindowInDays));
            
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
                    
                new (KnownDisplayTypes.Number, 
                    (bond.MaturityDate.ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days.ToString())
            ];
                
            // Вычисляем полную доходность облигации
            var nextCoupon = bondCoupons
                .FirstOrDefault(x => x.Ticker == bond.Ticker);
                
            double profitPrc = 0.0;
                
            if (nextCoupon is not null)
                profitPrc = (bond.LastPrice / (365.0 / nextCoupon.CouponPeriod) * nextCoupon.PayOneBond) / 100.0;
                
            data.Add(new ReportParameter(
                KnownDisplayTypes.Percent, 
                profitPrc.ToString("N1")));
                
            foreach (var date in dates)
            {
                var bondCoupon = bondCoupons
                    .FirstOrDefault(x => 
                        x.Ticker == bond.Ticker &&
                        x.CouponDate.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(bondCoupon is not null 
                    ? new ReportParameter(
                        KnownDisplayTypes.Ruble, 
                        bondCoupon.PayOneBond.ToString("N1")) 
                    : new ReportParameter(
                        KnownDisplayTypes.Ruble, 
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }

    /// <inheritdoc />
    public async Task<ReportData> GetReportBondsAnalyseSupertrendAsync(
        GetAnalyseRequest request) =>
        await GetReportDataBondsByAnalyseType(
            await bondRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Supertrend);

    /// <inheritdoc />
    public async Task<ReportData> GetReportBondsAnalyseCandleSequenceAsync(
        GetAnalyseRequest request) =>
        await GetReportDataBondsByAnalyseType(
            await bondRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.CandleSequence);

    #endregion
    

    
    #region Spreads
    
    /// <inheritdoc />
    public async Task<ReportData> GetReportSpreadsAsync()
    {
        var spreads = await spreadRepository
            .GetWatchListAsync();
            
        var reportData = new ReportData
        {
            Title = "Спреды",
            Header =
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер 1"),
                new ReportParameter(KnownDisplayTypes.String, "Цена"),
                new ReportParameter(KnownDisplayTypes.String, "Описание"),
                new ReportParameter(KnownDisplayTypes.String, "Тикер 2"),
                new ReportParameter(KnownDisplayTypes.String, "Цена"),
                new ReportParameter(KnownDisplayTypes.String, "Описание"),
                new ReportParameter(KnownDisplayTypes.String, "Спред"),
                new ReportParameter(KnownDisplayTypes.String, "Спред, %"),
                new ReportParameter(KnownDisplayTypes.String, "Фандинг")
            ]
        };

        foreach (var spread in spreads)
        {
            List<ReportParameter> data =
            [
                new (KnownDisplayTypes.Ticker, spread.FirstInstrumentTicker),
                new (KnownDisplayTypes.Ruble, spread.FirstInstrumentPrice.ToString("N5")),
                new (KnownDisplayTypes.String, spread.FirstInstrumentRole),
                new (KnownDisplayTypes.Ticker, spread.SecondInstrumentTicker),
                new (KnownDisplayTypes.Ruble, spread.SecondInstrumentPrice.ToString("N5")),
                new (KnownDisplayTypes.String, spread.SecondInstrumentRole),
                new (KnownDisplayTypes.Ruble, spread.PriceDifference.ToString("N5")),
                new (KnownDisplayTypes.Percent, spread.PriceDifferencePrc.ToString("N5")),
                new (KnownDisplayTypes.Ruble, spread.Funding.ToString("N5"))
            ];
            
            reportData.Data.Add(data);
        }
        
        return reportData;
    }

    #endregion
    
    #region Currencies
    
    /// <inheritdoc />
    public async Task<ReportData> GetReportCurrenciesAnalyseSupertrendAsync(
        GetAnalyseRequest request) =>
        await GetReportDataCurrenciesByAnalyseType(
            await currencyRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Supertrend);

    /// <inheritdoc />
    public async Task<ReportData> GetReportCurrenciesAnalyseCandleSequenceAsync(
        GetAnalyseRequest request) =>
        await GetReportDataCurrenciesByAnalyseType(
            await currencyRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.CandleSequence);

    #endregion
    
    #region Futures
    
    /// <inheritdoc />
    public async Task<ReportData> GetReportFuturesAnalyseRsiAsync(
        GetAnalyseRequest request) =>
        await GetReportDataFuturesByAnalyseType(
            await futureRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Rsi);

    /// <inheritdoc />
    public async Task<ReportData> GetReportFuturesAnalyseCandleVolumeAsync(
        GetAnalyseRequest request) =>
        await GetReportDataFuturesByAnalyseType(
            await futureRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.CandleVolume);

    /// <inheritdoc />
    public async Task<ReportData> GetReportFuturesAnalyseCandleSequenceAsync(
        GetAnalyseRequest request) =>
        await GetReportDataFuturesByAnalyseType(
            await futureRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.CandleSequence);

    /// <inheritdoc />
    public async Task<ReportData> GetReportFuturesAnalyseSupertrendAsync(
        GetAnalyseRequest request) =>
        await GetReportDataFuturesByAnalyseType(
            await futureRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Supertrend);

    #endregion
    
    #region Indexes
    
    /// <inheritdoc />
    public async Task<ReportData> GetReportIndexesAnalyseSupertrendAsync(
        GetAnalyseRequest request) =>
        await GetReportDataIndexesByAnalyseType(
            await indexRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Supertrend);

    /// <inheritdoc />
    public async Task<ReportData> GetReportIndexesAnalyseCandleSequenceAsync(
        GetAnalyseRequest request) =>
        await GetReportDataIndexesByAnalyseType(
            await indexRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.CandleSequence);

    /// <inheritdoc />
    public async Task<ReportData> GetReportIndexesAnalyseYieldLtmAsync(
        GetAnalyseRequest request) =>
        await GetReportDataIndexesByAnalyseType(
            await indexRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.YieldLtm);
    
    #endregion
    
    private List<ReportParameter> GetDates(
        DateOnly from, DateOnly to)
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
        

    

    
    private async Task<ReportData> GetReportDataFuturesByAnalyseType(
        List<Future> futures,
        DateOnly from,
        DateOnly to,
        string analyseType)
    {
        var instrumentIds = futures
            .Select(x => x.InstrumentId)
            .ToList();        
        
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => x.AnalyseType == analyseType)
            .ToList();
            
        var dates = GetDates(from, to);
            
        var reportData = new ReportData
        {
            Title = $"Анализ {analyseType} " +
                    $"с {from.ToString(KnownDateTimeFormats.DateISO)} " +
                    $"по {to.ToString(KnownDateTimeFormats.DateISO)}",
                
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер")
            ]
        };

        reportData.Header.AddRange(dates);

        foreach (var future in futures)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, future.Ticker)
            };

            foreach (var date in dates)
            {
                var analyseResult = analyseResults
                    .FirstOrDefault(x => 
                        x.InstrumentId == future.InstrumentId && 
                        x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(analyseResult is not null 
                    ? new ReportParameter(
                        $"AnalyseResult{analyseType}",
                        analyseResult.Result) 
                    : new ReportParameter(
                        $"AnalyseResult{analyseType}",
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
    
    private async Task<ReportData> GetReportDataBondsByAnalyseType(
        List<Bond> bonds,
        DateOnly from,
        DateOnly to,
        string analyseType)
    {
        var instrumentIds = bonds
            .Select(x => x.InstrumentId)
            .ToList();        
        
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => x.AnalyseType == analyseType)
            .ToList();
            
        var dates = GetDates(from, to);
            
        var reportData = new ReportData
        {
            Title = $"Анализ {analyseType} " +
                    $"с {from.ToString(KnownDateTimeFormats.DateISO)} " +
                    $"по {to.ToString(KnownDateTimeFormats.DateISO)}",
                
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер")
            ]
        };

        reportData.Header.AddRange(dates);

        foreach (var bond in bonds)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, bond.Ticker)
            };

            foreach (var date in dates)
            {
                var analyseResult = analyseResults
                    .FirstOrDefault(x => 
                        x.InstrumentId == bond.InstrumentId && 
                        x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(analyseResult is not null 
                    ? new ReportParameter(
                        $"AnalyseResult{analyseType}",
                        analyseResult.Result) 
                    : new ReportParameter(
                        $"AnalyseResult{analyseType}",
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
    
    private async Task<ReportData> GetReportDataCurrenciesByAnalyseType(
        List<Currency> currencies,
        DateOnly from,
        DateOnly to,
        string analyseType)
    {
        var instrumentIds = currencies
            .Select(x => x.InstrumentId)
            .ToList();        
        
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => x.AnalyseType == analyseType)
            .ToList();
            
        var dates = GetDates(from, to);
            
        var reportData = new ReportData
        {
            Title = $"Анализ {analyseType} " +
                    $"с {from.ToString(KnownDateTimeFormats.DateISO)} " +
                    $"по {to.ToString(KnownDateTimeFormats.DateISO)}",
                
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер")
            ]
        };

        reportData.Header.AddRange(dates);

        foreach (var currency in currencies)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, currency.Ticker)
            };

            foreach (var date in dates)
            {
                var analyseResult = analyseResults
                    .FirstOrDefault(x => 
                        x.InstrumentId == currency.InstrumentId && 
                        x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(analyseResult is not null 
                    ? new ReportParameter(
                        $"AnalyseResult{analyseType}",
                        analyseResult.Result) 
                    : new ReportParameter(
                        $"AnalyseResult{analyseType}",
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
}