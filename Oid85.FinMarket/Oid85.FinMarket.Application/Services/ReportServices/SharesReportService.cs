using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Services.ReportServices;

/// <inheritdoc />
public class SharesReportService(
    ITickerListUtilService tickerListUtilService,
    IReportDataFactory reportDataFactory)
    : ISharesReportService
{
    private async Task<List<Guid>> GetInstrumentIds(string tickerList) =>
        (await tickerListUtilService.GetSharesByTickerListAsync(tickerList))
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
    public async Task<ReportData> GetRsiAnalyseAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(request.TickerList), 
            KnownAnalyseTypes.Rsi, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetYieldLtmAnalyseAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(request.TickerList), 
            KnownAnalyseTypes.YieldLtm, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetDrawdownFromMaximumAnalyseAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(request.TickerList), 
            KnownAnalyseTypes.DrawdownFromMaximum, 
            request.From, request.To);

    /// <inheritdoc />
    public async Task<ReportData> GetDividendAnalyseAsync(TickerListRequest request) =>
        await reportDataFactory.CreateDividendInfoReportDataAsync(
            await GetInstrumentIds(request.TickerList));
    
    /// <inheritdoc />
    public async Task<ReportData> GetMultiplicatorAnalyseAsync(TickerListRequest request) =>
        await reportDataFactory.CreateMultiplicatorReportDataAsync(
            await GetInstrumentIds(request.TickerList));

    /// <inheritdoc />
    public async Task<ReportData> GetForecastTargetAnalyseAsync(TickerListRequest request) =>
        await reportDataFactory.CreateForecastTargetReportDataAsync(
            await GetInstrumentIds(request.TickerList));

    /// <inheritdoc />
    public async Task<ReportData> GetForecastConsensusAnalyseAsync(TickerListRequest request) =>
        await reportDataFactory.CreateForecastConsensusReportDataAsync(
            await GetInstrumentIds(request.TickerList));
    
    /// <inheritdoc />
    public async Task<ReportData> GetActiveMarketEventsAnalyseAsync(TickerListRequest request) => 
        await reportDataFactory.CreateActiveMarketEventsReportDataAsync(
            await GetInstrumentIds(request.TickerList));

    /// <inheritdoc />
    public async Task<ReportData> GetAssetReportEventsAnalyseAsync(TickerListRequest request) => 
        await reportDataFactory.CreateAssetReportEventsReportDataAsync(
            await GetInstrumentIds(request.TickerList));

    /// <inheritdoc />
    public async Task<ReportData> GetFearGreedIndexAsync(DateRangeRequest request) =>
        await reportDataFactory.CreateFearGreedIndexReportDataAsync(request.From, request.To);
}