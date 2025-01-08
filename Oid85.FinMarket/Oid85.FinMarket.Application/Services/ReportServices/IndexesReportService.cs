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
    public async Task<ReportData> GetAggregatedAnalyseAsync(GetAnalyseRequest request) =>
        await GetReportDataAggregatedAnalyse(
            await indexRepository.GetWatchListAsync(), 
            request.From, 
            request.To);

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
                        analyseResult.ResultString) 
                    : new ReportParameter(
                        $"AnalyseResult{analyseType}",
                        string.Empty));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
    
    private async Task<ReportData> GetReportDataAggregatedAnalyse(
        List<FinIndex> instruments,
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
                    resultNumber.ToString("N2")));
            }
                
            reportData.Data.Add(data);
        }
            
        return reportData;
    }
}