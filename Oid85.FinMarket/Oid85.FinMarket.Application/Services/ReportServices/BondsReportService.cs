﻿using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Services.ReportServices;

/// <inheritdoc />
public class BondsReportService(
    ITickerListUtilService tickerListUtilService,
    IReportDataFactory reportDataFactory) 
    : IBondsReportService
{
    private async Task<List<Guid>> GetInstrumentIds(string tickerList) =>
        (await tickerListUtilService.GetBondsByTickerListAsync(tickerList))
        .OrderBy(x => x.Sector).Select(x => x.InstrumentId).ToList();
    
    /// <inheritdoc />
    public async Task<ReportData> GetAggregatedAnalyseAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateAggregatedReportDataAsync(
            await GetInstrumentIds(request.TickerList), 
            [
                KnownAnalyseTypes.Supertrend,
                KnownAnalyseTypes.CandleSequence,
                KnownAnalyseTypes.CandleVolume
            ], 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetSupertrendAnalyseAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(request.TickerList), 
            KnownAnalyseTypes.Supertrend, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleSequenceAnalyseAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(request.TickerList), 
            KnownAnalyseTypes.CandleSequence, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetCandleVolumeAnalyseAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(request.TickerList), 
            KnownAnalyseTypes.CandleVolume, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetCouponAnalyseAsync(TickerListRequest request) =>
        await reportDataFactory.CreateBondCouponReportDataAsync(
            await GetInstrumentIds(request.TickerList));

    /// <inheritdoc />
    public async Task<ReportData> GetBondSelectionAsync() =>
        await reportDataFactory.CreateBondCouponReportDataAsync(
            (await tickerListUtilService.GetBondsByFilter())
            .Select(x => x.InstrumentId).ToList());

    /// <inheritdoc />
    public async Task<ReportData> GetActiveMarketEventsAnalyseAsync(TickerListRequest request) => 
        await reportDataFactory.CreateActiveMarketEventsReportDataAsync(
            await GetInstrumentIds(request.TickerList));
}