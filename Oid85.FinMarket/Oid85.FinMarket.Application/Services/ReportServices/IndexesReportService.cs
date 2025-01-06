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
public class IndexesReportService(
    IConfiguration configuration,
    IAnalyseResultRepository analyseResultRepository,
    IIndexRepository indexRepository,
    ReportHelper reportHelper) 
    : IIndexesReportService
{
    /// <inheritdoc />
    public async Task<ReportData> GetAggregatedAnalyseAsync(GetAnalyseByTickerRequest request)
    {
        var instrument = await indexRepository.GetByTickerAsync(request.Ticker);
            
        if (instrument is null)
            return new ();
            
        var reportData = new ReportData();

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
            await indexRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Supertrend);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleSequenceAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await indexRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.CandleSequence);

    /// <inheritdoc />
    public async Task<ReportData> GetRsiAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await indexRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.Rsi);

    /// <inheritdoc />
    public async Task<ReportData> GetYieldLtmAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataByAnalyseType(
            await indexRepository.GetWatchListAsync(),
            request.From,
            request.To,
            KnownAnalyseTypes.YieldLtm);
    
    private async Task<ReportData> GetReportDataByAnalyseType(
        List<FinIndex> instruments,
        DateOnly from,
        DateOnly to,
        string analyseType)
    {
        var instrumentIds = instruments
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