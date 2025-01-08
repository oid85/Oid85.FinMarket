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
public class CurrenciesReportService(
    IConfiguration configuration,
    IAnalyseResultRepository analyseResultRepository,
    ICurrencyRepository currencyRepository,
    ReportHelper reportHelper) 
    : ICurrenciesReportService
{
    /// <inheritdoc />
    public async Task<ReportData> GetAggregatedAnalyseAsync(GetAnalyseByTickerRequest request)
    {
        var reportData = new ReportData();
        
        if (string.IsNullOrEmpty(request.Ticker))
            request.Ticker = (await currencyRepository.GetWatchListAsync()).FirstOrDefault()?.Ticker ?? string.Empty;
        
        if (string.IsNullOrEmpty(request.Ticker))
            return new ();
        
        var instrument = await currencyRepository.GetByTickerAsync(request.Ticker);
            
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
                [instrument], request.From, request.To, KnownAnalyseTypes.Rsi))
            .Data.First(),
            
            (await GetReportDataByAnalyseType(
                [instrument], request.From, request.To, KnownAnalyseTypes.YieldLtm))
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
            await currencyRepository.GetWatchListAsync(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.Supertrend);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleSequenceAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await currencyRepository.GetWatchListAsync(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.CandleSequence);

    /// <inheritdoc />
    public async Task<ReportData> GetRsiAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await currencyRepository.GetWatchListAsync(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.Rsi);

    /// <inheritdoc />
    public async Task<ReportData> GetYieldLtmAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await currencyRepository.GetWatchListAsync(), 
            request.From, 
            request.To, 
            KnownAnalyseTypes.YieldLtm);
    
    private async Task<ReportData> GetReportDataByAnalyseType(
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
                        analyseResult.ResultString) 
                    : new ReportParameter(
                        $"AnalyseResult{analyseType}",
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
}