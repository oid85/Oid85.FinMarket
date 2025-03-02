using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services.ReportServices;

/// <inheritdoc />
public class IndexesReportService(
    IAnalyseResultRepository analyseResultRepository,
    ReportHelper reportHelper,
    IInstrumentService instrumentService,
    IReportDataFactory reportDataFactory) 
    : IIndexesReportService
{
    private async Task<List<Guid>> GetInstrumentIds() =>
        (await instrumentService.GetFuturesInWatchlist()).Select(x => x.InstrumentId).ToList();
    
    /// <inheritdoc />
    public async Task<ReportData> GetAggregatedAnalyseAsync(GetAnalyseRequest request) =>
        await reportDataFactory.CreateAggregatedReportDataAsync(
            await GetInstrumentIds(), 
            [
                KnownAnalyseTypes.Supertrend,
                KnownAnalyseTypes.CandleSequence,
                KnownAnalyseTypes.CandleVolume,
                KnownAnalyseTypes.Rsi
            ], 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetSupertrendAnalyseAsync(GetAnalyseRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(), 
            KnownAnalyseTypes.Supertrend, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleSequenceAnalyseAsync(GetAnalyseRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(), 
            KnownAnalyseTypes.CandleSequence, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetRsiAnalyseAsync(GetAnalyseRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(), 
            KnownAnalyseTypes.Rsi, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetYieldLtmAnalyseAsync(GetAnalyseRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(), 
            KnownAnalyseTypes.YieldLtm, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetDrawdownFromMaximumAnalyseAsync(GetAnalyseRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(), 
            KnownAnalyseTypes.DrawdownFromMaximum, 
            request.From, request.To);
}