﻿using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Services.ReportServices;

/// <inheritdoc />
public class SharesReportService(
    IInstrumentService instrumentService,
    IReportDataFactory reportDataFactory)
    : ISharesReportService
{
    private async Task<List<Guid>> GetInstrumentIds() =>
        (await instrumentService.GetSharesInWatchlist())
        .OrderBy(x => x.Sector).Select(x => x.InstrumentId).ToList();
    
    /// <inheritdoc />
    public async Task<ReportData> GetAggregatedAnalyseAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateAggregatedReportDataAsync(
            await GetInstrumentIds(), 
            [
                KnownAnalyseTypes.Supertrend,
                KnownAnalyseTypes.CandleSequence,
                KnownAnalyseTypes.CandleVolume
            ], 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetSupertrendAnalyseAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(), 
            KnownAnalyseTypes.Supertrend, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleSequenceAnalyseAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(), 
            KnownAnalyseTypes.CandleSequence, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleVolumeAnalyseAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(), 
            KnownAnalyseTypes.CandleVolume, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetRsiAnalyseAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(), 
            KnownAnalyseTypes.Rsi, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetYieldLtmAnalyseAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(), 
            KnownAnalyseTypes.YieldLtm, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetDrawdownFromMaximumAnalyseAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(), 
            KnownAnalyseTypes.DrawdownFromMaximum, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetDividendAnalyseAsync() =>
        await reportDataFactory.CreateDividendInfoReportDataAsync();
    
    /// <inheritdoc />
    public async Task<ReportData> GetMultiplicatorAnalyseAsync() =>
        await reportDataFactory.CreateMultiplicatorReportDataAsync();

    /// <inheritdoc />
    public async Task<ReportData> GetForecastTargetAnalyseAsync() =>
        await reportDataFactory.CreateForecastTargetReportDataAsync();

    /// <inheritdoc />
    public async Task<ReportData> GetForecastConsensusAnalyseAsync() =>
        await reportDataFactory.CreateForecastConsensusReportDataAsync();
    
    /// <inheritdoc />
    public async Task<ReportData> GetActiveMarketEventsAnalyseAsync() => 
        await reportDataFactory.CreateActiveMarketEventsReportDataAsync(
            await GetInstrumentIds());

    /// <inheritdoc />
    public async Task<ReportData> GetAssetReportEventsAnalyseAsync() => 
        await reportDataFactory.CreateAssetReportEventsReportDataAsync(
            await GetInstrumentIds());

    /// <inheritdoc />
    public async Task<ReportData> GetFearGreedIndexAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateFearGreedIndexReportDataAsync(request.From, request.To);
}