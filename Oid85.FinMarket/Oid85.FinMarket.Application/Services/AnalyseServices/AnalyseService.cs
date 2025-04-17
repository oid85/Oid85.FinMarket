using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Services.AnalyseServices;

/// <inheritdoc />
public class AnalyseService(
    ITickerListUtilService tickerListUtilService,
    CandleSequenceAnalyseService candleSequenceAnalyseService,
    CandleVolumeAnalyseService candleVolumeAnalyseService,
    DrawdownFromMaximumAnalyseService drawdownFromMaximumAnalyseService,
    RsiAnalyseService rsiAnalyseService,
    SupertrendAnalyseService supertrendAnalyseService,
    YieldLtmAnalyseService yieldLtmAnalyseService)
    : IAnalyseService
{
    /// <inheritdoc />
    public async Task<bool> DailyAnalyseSharesAsync()
    {
        var instruments = await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.SharesWatchlist);

        foreach (var instrument in instruments)
        {
            // Вызов методов анализа
            await supertrendAnalyseService.SupertrendAnalyseAsync(instrument.InstrumentId);
            await candleSequenceAnalyseService.CandleSequenceAnalyseAsync(instrument.InstrumentId);
            await candleVolumeAnalyseService.CandleVolumeAnalyseAsync(instrument.InstrumentId);
            await rsiAnalyseService.RsiAnalyseAsync(instrument.InstrumentId);
            await yieldLtmAnalyseService.YieldLtmAnalyseAsync(instrument.InstrumentId);
            await drawdownFromMaximumAnalyseService.DrawdownFromMaximumAnalyseAsync(instrument.InstrumentId);
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> DailyAnalyseBondsAsync()
    {
        var instruments = await tickerListUtilService.GetBondsByTickerListAsync(KnownTickerLists.BondsWatchlist);
        
        foreach (var instrument in instruments)
        {
            // Вызов методов анализа
            await supertrendAnalyseService.SupertrendAnalyseAsync(instrument.InstrumentId);
            await candleSequenceAnalyseService.CandleSequenceAnalyseAsync(instrument.InstrumentId);
            await candleVolumeAnalyseService.CandleVolumeAnalyseAsync(instrument.InstrumentId);
        }            
            
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> DailyAnalyseCurrenciesAsync()
    {
        var instruments = await tickerListUtilService.GetCurrenciesByTickerListAsync(KnownTickerLists.CurrenciesWatchlist);
        
        foreach (var instrument in instruments)
        {
            // Вызов методов анализа
            await supertrendAnalyseService.SupertrendAnalyseAsync(instrument.InstrumentId);
            await candleSequenceAnalyseService.CandleSequenceAnalyseAsync(instrument.InstrumentId);
            await rsiAnalyseService.RsiAnalyseAsync(instrument.InstrumentId);
            await yieldLtmAnalyseService.YieldLtmAnalyseAsync(instrument.InstrumentId);
            await drawdownFromMaximumAnalyseService.DrawdownFromMaximumAnalyseAsync(instrument.InstrumentId);
        }
            
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> DailyAnalyseFuturesAsync()
    {
        var instruments = await tickerListUtilService.GetFuturesByTickerListAsync(KnownTickerLists.FuturesWatchlist);
        
        foreach (var instrument in instruments)
        {
            // Вызов методов анализа
            await supertrendAnalyseService.SupertrendAnalyseAsync(instrument.InstrumentId);
            await candleSequenceAnalyseService.CandleSequenceAnalyseAsync(instrument.InstrumentId);
            await candleVolumeAnalyseService.CandleVolumeAnalyseAsync(instrument.InstrumentId);
            await rsiAnalyseService.RsiAnalyseAsync(instrument.InstrumentId);
            await yieldLtmAnalyseService.YieldLtmAnalyseAsync(instrument.InstrumentId);
        }
            
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> DailyAnalyseIndexesAsync()
    {
        var instruments = await tickerListUtilService.GetFinIndexesByTickerListAsync(KnownTickerLists.IndexesWatchlist);
        
        foreach (var instrument in instruments)
        {
            // Вызов методов анализа
            await supertrendAnalyseService.SupertrendAnalyseAsync(instrument.InstrumentId);
            await candleSequenceAnalyseService.CandleSequenceAnalyseAsync(instrument.InstrumentId);
            await rsiAnalyseService.RsiAnalyseAsync(instrument.InstrumentId);
            await yieldLtmAnalyseService.YieldLtmAnalyseAsync(instrument.InstrumentId);
            await drawdownFromMaximumAnalyseService.DrawdownFromMaximumAnalyseAsync(instrument.InstrumentId);
        }
            
        return true;
    }
}