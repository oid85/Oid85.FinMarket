using Microsoft.Extensions.Configuration;
using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services.ReportServices;

/// <inheritdoc />
public class BondsReportService(
    IConfiguration configuration,
    IAnalyseResultRepository analyseResultRepository,
    IBondCouponRepository bondCouponRepository,
    ReportHelper reportHelper,
    IInstrumentService instrumentService,
    IResourceStoreService resourceStoreService) 
    : IBondsReportService
{
    /// <inheritdoc />
    public async Task<ReportData> GetAggregatedAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataAggregatedAnalyse(
            await instrumentService.GetBondsInWatchlist(), 
            request.From, 
            request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetSupertrendAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await instrumentService.GetBondsInWatchlist(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.Supertrend);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleSequenceAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await instrumentService.GetBondsInWatchlist(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.CandleSequence);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleVolumeAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await instrumentService.GetBondsInWatchlist(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.CandleVolume);

    /// <inheritdoc />
    public async Task<ReportData> GetCouponAnalyseAsync()
    {
        var bonds = await instrumentService.GetBondsInWatchlist();
            
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

            string color = (await resourceStoreService.GetColorPaletteYieldCouponAsync())
                .FirstOrDefault(x => 
                    profitPrc >= x.LowLevel && 
                    profitPrc >= x.HighLevel)!
                .ColorCode;
            
            data.Add(new ReportParameter(
                KnownDisplayTypes.Percent, 
                profitPrc.ToString("N1"),
                color));
                
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
                        analyseResult.ResultString,
                        reportHelper.GetColor(
                            analyseType, 
                            analyseResult)) 
                    : new ReportParameter(
                        $"AnalyseResult{analyseType}",
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
    
    private async Task<ReportData> GetReportDataAggregatedAnalyse(
        List<Bond> instruments,
        DateOnly from,
        DateOnly to)
    {
        var instrumentIds = instruments
            .Select(x => x.InstrumentId)
            .ToList();        
        
        var analyseTypes = new List<string>()
        {
            KnownAnalyseTypes.Supertrend,
            KnownAnalyseTypes.CandleSequence,
            KnownAnalyseTypes.CandleVolume
        };
            
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => analyseTypes.Contains(x.AnalyseType))
            .ToList();
        
        var dates = reportHelper.GetDates(from, to);
            
        var reportData = new ReportData
        {
            Title = $"Анализ Aggregated " +
                    $"с {from.ToString(KnownDateTimeFormats.DateISO)} " +
                    $"по {to.ToString(KnownDateTimeFormats.DateISO)}",
                
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер")
            ]
        };

        reportData.Header.AddRange(dates);

        foreach (var instrument in instruments)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, instrument.Ticker)
            };

            foreach (var date in dates)
            {
                double resultNumber = analyseResults
                    .Where(x => 
                        x.InstrumentId == instrument.InstrumentId && 
                        analyseTypes.Contains(x.AnalyseType) &&
                        x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value)
                    .Select(x => x.ResultNumber)
                    .Sum();
                
                data.Add(new ReportParameter(
                    $"AnalyseResult{KnownAnalyseTypes.Aggregated}",
                    resultNumber.ToString("N0"),
                    reportHelper.GetColor(
                        KnownAnalyseTypes.Aggregated, 
                        new AnalyseResult { ResultNumber = resultNumber})));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
}