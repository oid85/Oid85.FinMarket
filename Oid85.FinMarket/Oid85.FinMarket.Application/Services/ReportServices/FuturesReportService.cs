using Oid85.FinMarket.Application.Helpers;
using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.ReportServices;
using Oid85.FinMarket.Application.Models.Reports;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Services.ReportServices;

/// <inheritdoc />
public class FuturesReportService(
    ISpreadRepository spreadRepository,
    ReportHelper reportHelper,
    IInstrumentService instrumentService,
    IReportDataFactory reportDataFactory)
    : IFuturesReportService
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
    public async Task<ReportData> GetCandleVolumeAnalyseAsync(GetAnalyseRequest request) =>
        await reportDataFactory.CreateReportDataAsync(
            await GetInstrumentIds(), 
            KnownAnalyseTypes.CandleVolume, 
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
                new(KnownDisplayTypes.String, spread.FirstInstrumentRole),
                new(KnownDisplayTypes.String, spread.SecondInstrumentRole),
                new(KnownDisplayTypes.Ticker, spread.FirstInstrumentTicker),
                new(KnownDisplayTypes.Ticker, spread.SecondInstrumentTicker),
                new(KnownDisplayTypes.Ruble, spread.FirstInstrumentPrice.ToString("N2")),
                new(KnownDisplayTypes.Ruble, spread.SecondInstrumentPrice.ToString("N2")),
                new(KnownDisplayTypes.Ruble, spread.PriceDifference.ToString("N2")),
                new(KnownDisplayTypes.Percent, spread.PriceDifferencePrc.ToString("N2")),
                new(KnownDisplayTypes.String, spread.SpreadPricePosition,
                    await reportHelper.GetColorPaletteSpreadPricePosition(spread.SpreadPricePosition))
            ];

            reportData.Data.Add(data);
        }
        
        return reportData;
    }
}