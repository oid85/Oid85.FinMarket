﻿using Oid85.FinMarket.Application.Helpers;
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
public class FuturesReportService(
    IAnalyseResultRepository analyseResultRepository,
    ISpreadRepository spreadRepository,
    ReportHelper reportHelper,
    IInstrumentService instrumentService,
    IResourceStoreService resourceStoreService)
    : IFuturesReportService
{
    public async Task<ReportData> GetAggregatedAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataAggregatedAnalyse(
            await instrumentService.GetFuturesInWatchlist(), 
            request.From, 
            request.To);

    public async Task<ReportData> GetSupertrendAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await instrumentService.GetFuturesInWatchlist(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.Supertrend);

    public async Task<ReportData> GetCandleSequenceAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await instrumentService.GetFuturesInWatchlist(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.CandleSequence);

    public async Task<ReportData> GetCandleVolumeAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await instrumentService.GetFuturesInWatchlist(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.CandleVolume);

    public async Task<ReportData> GetRsiAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await instrumentService.GetFuturesInWatchlist(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.Rsi);

    /// <inheritdoc />
    public async Task<ReportData> GetYieldLtmAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataYieldLtmAnalyse(
            await instrumentService.GetFuturesInWatchlist(), 
            request.From, 
            request.To);

    public async Task<ReportData> GetSpreadAnalyseAsync()
    {
        var spreads = await spreadRepository.GetAllAsync();
            
        var reportData = new ReportData
        {
            Title = "Спреды",
            Header =
            [
                new ReportParameter(KnownDisplayTypes.String, "Первый"),
                new ReportParameter(KnownDisplayTypes.String, "Второй"),
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Тикер"),
                new ReportParameter(KnownDisplayTypes.String, "Цена"),
                new ReportParameter(KnownDisplayTypes.String, "Цена"),
                new ReportParameter(KnownDisplayTypes.String, "Спред"),
                new ReportParameter(KnownDisplayTypes.String, "Спред, %"),
                new ReportParameter(KnownDisplayTypes.String, "Конт./Бэкв.")
            ]
        };

        foreach (var spread in spreads)
        {
            List<ReportParameter> data =
            [
                new (KnownDisplayTypes.String, spread.FirstInstrumentRole),
                new (KnownDisplayTypes.String, spread.SecondInstrumentRole),
                new (KnownDisplayTypes.Ticker, spread.FirstInstrumentTicker),
                new (KnownDisplayTypes.Ticker, spread.SecondInstrumentTicker),
                new (KnownDisplayTypes.Ruble, spread.FirstInstrumentPrice.ToString("N2")),
                new (KnownDisplayTypes.Ruble, spread.SecondInstrumentPrice.ToString("N2")),
                new (KnownDisplayTypes.Ruble, spread.PriceDifference.ToString("N2")),
                new (KnownDisplayTypes.Percent, spread.PriceDifferencePrc.ToString("N2"))
            ];
            
            string color = (await resourceStoreService.GetColorPaletteSpreadPricePositionAsync())
                .FirstOrDefault(x => 
                    spread.SpreadPricePosition == x.Value)!
                .ColorCode; 
            
            data.Add(new (
                KnownDisplayTypes.String, 
                spread.SpreadPricePosition,
                color));
            
            reportData.Data.Add(data);
        }
        
        return reportData;
    }
    
    private async Task<ReportData> GetReportDataByAnalyseType(
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
                        analyseResult.ResultString,
                        await reportHelper.GetColor(
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
        List<Future> instruments,
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
            KnownAnalyseTypes.CandleVolume,
            KnownAnalyseTypes.Rsi
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
                    await  reportHelper.GetColor(
                        KnownAnalyseTypes.Aggregated, 
                        new AnalyseResult { ResultNumber = resultNumber})));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }    
    
    private async Task<ReportData> GetReportDataYieldLtmAnalyse(
        List<Future> instruments, 
        DateOnly from, 
        DateOnly to)
    {
        var instrumentIds = instruments
            .Select(x => x.InstrumentId)
            .ToList();        
        
        var analyseResults = (await analyseResultRepository
                .GetAsync(instrumentIds, from, to))
            .Where(x => x.AnalyseType == KnownAnalyseTypes.YieldLtm)
            .ToList();
        
        var dates = reportHelper.GetDates(from, to);
            
        var reportData = new ReportData
        {
            Title = $"Анализ {KnownAnalyseTypes.YieldLtm} " +
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
                var analyseResult = analyseResults
                    .FirstOrDefault(x =>
                        x.InstrumentId == instrument.InstrumentId &&
                        x.Date.ToString(KnownDateTimeFormats.DateISO) == date.Value);

                data.Add(analyseResult is not null
                    ? new ReportParameter(
                        KnownDisplayTypes.Percent,
                        analyseResult.ResultString,
                        await reportHelper.GetColor(
                            KnownAnalyseTypes.YieldLtm, 
                            analyseResult))
                    : new ReportParameter(
                        KnownDisplayTypes.Percent,
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
}