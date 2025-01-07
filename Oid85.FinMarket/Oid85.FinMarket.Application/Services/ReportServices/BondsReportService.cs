using Microsoft.Extensions.Configuration;
using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services.ReportServices;

/// <inheritdoc />
public class BondsReportService(
    IConfiguration configuration,
    IAnalyseResultRepository analyseResultRepository,
    IBondRepository bondRepository,
    IBondCouponRepository bondCouponRepository,
    ReportHelper reportHelper) 
    : IBondsReportService
{
    /// <inheritdoc />
    public async Task<ReportData> GetAggregatedAnalyseAsync(GetAnalyseByTickerRequest request)
    {
        var reportData = new ReportData();
        
        if (string.IsNullOrEmpty(request.Ticker))
            request.Ticker = (await bondRepository.GetWatchListAsync()).FirstOrDefault()?.Ticker ?? string.Empty;
        
        if (string.IsNullOrEmpty(request.Ticker))
            return new ();
        
        var instrument = await bondRepository.GetByTickerAsync(request.Ticker);
            
        if (instrument is null)
            return new ();

        int outputWindowInDays = configuration
            .GetValue<int>(KnownSettingsKeys.ApplicationSettingsOutputWindowInDays);
        
        var dates = reportHelper
            .GetDates(request.From, request.To.AddDays(outputWindowInDays));

        reportData.Header =
        [
            new ReportParameter(KnownDisplayTypes.String, "Тикер"),
            new ReportParameter(KnownDisplayTypes.String, "Сектор")
        ];
            
        reportData.Header.AddRange(dates);

        reportData.Data = 
        [
            (await GetReportDataByAnalyseType(
                [instrument], request.From, request.To, KnownAnalyseTypes.Supertrend))
            .Data.First(),
                
            (await GetReportDataByAnalyseType(
                [instrument], request.From, request.To, KnownAnalyseTypes.CandleSequence))
            .Data.First(),
                
            (await GetReportDataByAnalyseType(
                [instrument], request.From, request.To, KnownAnalyseTypes.CandleVolume))
            .Data.First()
        ];
            
        reportData.Title =
            $"Анализ {request.Ticker} " +
            $"с {request.From.ToString(KnownDateTimeFormats.DateISO)} " +
            $"по {request.To.ToString(KnownDateTimeFormats.DateISO)}";
            
        return reportData;
    }

    /// <inheritdoc />
    public async Task<ReportData> GetSupertrendAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await bondRepository.GetWatchListAsync(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.Supertrend);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleSequenceAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await bondRepository.GetWatchListAsync(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.CandleSequence);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleVolumeAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await bondRepository.GetWatchListAsync(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.CandleVolume);

    /// <inheritdoc />
    public async Task<ReportData> GetCouponAnalyseAsync()
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
            
        int outputWindowInDays = configuration
            .GetValue<int>(KnownSettingsKeys.ApplicationSettingsOutputWindowInDays);
        
        var bondCoupons = await bondCouponRepository
            .GetAsync(
                DateOnly.FromDateTime(DateTime.Today), 
                DateOnly.FromDateTime(DateTime.Today).AddDays(outputWindowInDays));
            
        var dates = reportHelper.GetDates(
            DateOnly.FromDateTime(DateTime.Today), 
            DateOnly.FromDateTime(DateTime.Today).AddDays(outputWindowInDays));
            
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
    
    private async Task<ReportData> GetReportDataByAnalyseType(
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
            
        var dates = reportHelper.GetDates(from, to);
            
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
}