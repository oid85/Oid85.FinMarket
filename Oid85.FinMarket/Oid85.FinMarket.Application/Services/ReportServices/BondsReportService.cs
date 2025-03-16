using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Services.ReportServices;

/// <inheritdoc />
public class BondsReportService(
    IInstrumentService instrumentService,
    IReportDataFactory reportDataFactory) 
    : IBondsReportService
{
    private async Task<List<Guid>> GetInstrumentIds() =>
        (await instrumentService.GetBondsInWatchlist())
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
    public async Task<ReportData> GetCouponAnalyseAsync() =>
        await reportDataFactory.CreateBondCouponReportDataAsync(
            (await instrumentService.GetBondsInWatchlist())
            .Select(x => x.InstrumentId).ToList());

    /// <inheritdoc />
    public async Task<ReportData> GetBondSelectionAsync() =>
        await reportDataFactory.CreateBondCouponReportDataAsync(
            (await instrumentService.GetBondsByFilter())
            .Select(x => x.InstrumentId).ToList());

    /// <inheritdoc />
    public async Task<ReportData> GetActiveMarketEventsAnalyseAsync() => 
        await reportDataFactory.CreateActiveMarketEventsReportDataAsync(
            await GetInstrumentIds());
}