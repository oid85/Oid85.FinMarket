using Microsoft.Extensions.Configuration;
using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
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
    IBondCouponRepository bondCouponRepository,
    ReportHelper reportHelper,
    IInstrumentService instrumentService) 
    : IBondsReportService
{
    /// <inheritdoc />
    public async Task<ReportData> GetAggregatedAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataAggregatedAnalyse(await instrumentService.GetBondsInWatchlist(), request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetSupertrendAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await instrumentService.GetBondsInWatchlist(), request.From, request.To, KnownAnalyseTypes.Supertrend);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleSequenceAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await instrumentService.GetBondsInWatchlist(), request.From, request.To, KnownAnalyseTypes.CandleSequence);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleVolumeAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await instrumentService.GetBondsInWatchlist(), request.From, request.To, KnownAnalyseTypes.CandleVolume);

    /// <inheritdoc />
    public async Task<ReportData> GetCouponAnalyseAsync()
    {
        var bonds = await instrumentService.GetBondsInWatchlist();
        var reportData = await GetBondCouponReportDataAsync(bonds);
        return reportData;
    }

    /// <inheritdoc />
    public async Task<ReportData> GetBondSelectionAsync()
    {
        var bonds = await instrumentService.GetBondsByFilter();
        var reportData = await GetBondCouponReportDataAsync(bonds);
        return reportData;
    }
    
    private async Task<ReportData> GetBondCouponReportDataAsync(List<Bond> bonds)
    {
        var reportData = new ReportData
        {
            Title = "Информация по облигациям",
            Header =
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Наименование"),
                new ReportParameter(KnownDisplayTypes.String, "Сектор"),
                new ReportParameter(KnownDisplayTypes.String, "Плав. купон"),
                new ReportParameter(KnownDisplayTypes.String, "До погаш., дней"),
                new ReportParameter(KnownDisplayTypes.String, "Дох-ть., %")
            ]
        };
            
        int days = configuration.GetValue<int>(KnownSettingsKeys.ApplicationSettingsOutputWindowInDays);
        var instrumentIds = bonds.Select(x => x.InstrumentId).ToList();
        var startDate = DateOnly.FromDateTime(DateTime.Today);
        var endDate = DateOnly.FromDateTime(DateTime.Today).AddDays(days);

        var bondCoupons = (await bondCouponRepository
            .GetByInstrumentIdsAsync(instrumentIds))
            .Where(x =>
                x.CouponDate >= startDate &&
                x.CouponDate <= endDate)
            .ToList();

        var instrumentIdsWithCoupon = bondCoupons
            .Select(x => x.InstrumentId)
            .Distinct()
            .ToList();

        DateOnly GetNearCouponDate(Guid instrumentId)
        {
            var coupon = bondCoupons
                .OrderBy(x => x.CouponDate)
                .FirstOrDefault(x => x.InstrumentId == instrumentId);

            if (coupon is not null)
                return coupon.CouponDate;
            
            return DateOnly.MaxValue;
        }
        
        var selectedBonds = bonds
            .Where(x => instrumentIdsWithCoupon.Contains(x.InstrumentId))
            .OrderBy(x => GetNearCouponDate(x.InstrumentId));
        
        var dates = reportHelper.GetDates(startDate, endDate);
            
        reportData.Header.AddRange(dates);
            
        foreach (var bond in selectedBonds)
        {
            int daysToMaturityDate = (bond.MaturityDate.ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days;
            
            List<ReportParameter> data =
            [
                new (KnownDisplayTypes.Ticker, bond.Ticker),
                new (KnownDisplayTypes.String, bond.Name),                
                new (KnownDisplayTypes.Sector, bond.Sector),
                new (KnownDisplayTypes.String, bond.FloatingCouponFlag ? "Да" : string.Empty),
                new (KnownDisplayTypes.Number, daysToMaturityDate.ToString())
            ];
                
            // Вычисляем полную доходность облигации
            var nextCoupon = bondCoupons
                .OrderBy(x => x.CouponDate)
                .FirstOrDefault(x => x.InstrumentId == bond.InstrumentId);
                
            double profitPrc = 0.0;

            if (nextCoupon is not null &&
                bond.LastPrice == 0.0 &&
                nextCoupon.CouponPeriod == 0.0)
            {
                double priceInRubles = bond.LastPrice * 10.0;
                double payDay = nextCoupon.PayOneBond / nextCoupon.CouponPeriod;
                double payYear = payDay * 365;
                double profit = payYear / priceInRubles;
                profitPrc = profit / 100.0;   
            }
            
            data.Add(new ReportParameter(KnownDisplayTypes.Percent, profitPrc.ToString("N1"),
                await reportHelper.GetColorYieldCoupon(profitPrc)));
                
            foreach (var date in dates)
            {
                var bondCoupon = bondCoupons
                    .FirstOrDefault(x => 
                        x.InstrumentId == bond.InstrumentId &&
                        x.CouponDate.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(bondCoupon is not null 
                    ? new ReportParameter(KnownDisplayTypes.Ruble, bondCoupon.PayOneBond.ToString("N1")) 
                    : new ReportParameter(KnownDisplayTypes.Ruble, string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }

    private async Task<ReportData> GetReportDataByAnalyseType(
        List<Bond> bonds, DateOnly from, DateOnly to, string analyseType)
    {
        var instrumentIds = bonds.Select(x => x.InstrumentId).ToList();        
        
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => x.AnalyseType == analyseType)
            .ToList();
        
        var reportData = new ReportData
        {
            Title = $"Анализ {analyseType} " +
                    $"с {from.ToString(KnownDateTimeFormats.DateISO)} " +
                    $"по {to.ToString(KnownDateTimeFormats.DateISO)}",
                
            Header = 
            [
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Наименование"),
                new ReportParameter(KnownDisplayTypes.String, "Сектор")
            ]
        };
        
        var dates = reportHelper.GetDates(from, to);
        reportData.Header.AddRange(dates);

        foreach (var bond in bonds)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, bond.Ticker),
                new (KnownDisplayTypes.String, bond.Name),                
                new (KnownDisplayTypes.Sector, bond.Sector)
            };

            foreach (var date in dates)
            {
                var analyseResult = analyseResults
                    .FirstOrDefault(x => 
                        x.InstrumentId == bond.InstrumentId && 
                        x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(analyseResult is not null 
                    ? new ReportParameter($"AnalyseResult{analyseType}", analyseResult.ResultString,
                        await reportHelper.GetColorByAnalyseType(analyseType, analyseResult)) 
                    : new ReportParameter($"AnalyseResult{analyseType}", string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
    
    private async Task<ReportData> GetReportDataAggregatedAnalyse(
        List<Bond> instruments, DateOnly from, DateOnly to)
    {
        var instrumentIds = instruments.Select(x => x.InstrumentId).ToList();        
        
        var analyseTypes = new List<string>
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
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Наименование"),
                new ReportParameter(KnownDisplayTypes.String, "Сектор")
            ]
        };

        reportData.Header.AddRange(dates);

        foreach (var instrument in instruments)
        {
            var data = new List<ReportParameter>
            {
                new (KnownDisplayTypes.Ticker, instrument.Ticker),
                new (KnownDisplayTypes.String, instrument.Name),                
                new (KnownDisplayTypes.Sector, instrument.Sector)
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
                
                data.Add(new ReportParameter($"AnalyseResult{KnownAnalyseTypes.Aggregated}", resultNumber.ToString("N0"),
                    await reportHelper.GetColorAggregated((int) resultNumber)));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
}