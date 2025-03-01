﻿using NLog;
using Oid85.FinMarket.Application.Interfaces.Services;

namespace Oid85.FinMarket.Application.Services.AnalyseServices;

/// <inheritdoc />
public class AnalyseService(
    IInstrumentService instrumentService,
    CandleSequenceAnalyseService candleSequenceAnalyseService,
    CandleVolumeAnalyseService candleVolumeAnalyseService,
    DrawdownFromMaximumAnalyseService drawdownFromMaximumAnalyseService,
    RsiAnalyseService rsiAnalyseService,
    SupertrendAnalyseService supertrendAnalyseService,
    YieldLtmAnalyseService yieldLtmAnalyseService)
    : IAnalyseService
{
    /// <inheritdoc />
    public async Task<bool> AnalyseSharesAsync()
    {
        var instruments = await instrumentService.GetSharesInWatchlist();

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
    public async Task<bool> AnalyseBondsAsync()
    {
        var instruments = await instrumentService.GetBondsInWatchlist();
        
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
    public async Task<bool> AnalyseCurrenciesAsync()
    {
        var instruments = await instrumentService.GetCurrenciesInWatchlist();
        
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
    public async Task<bool> AnalyseFuturesAsync()
    {
        var instruments = await instrumentService.GetFuturesInWatchlist();
        
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
    public async Task<bool> AnalyseIndexesAsync()
    {
        var instruments = await instrumentService.GetFinIndexesInWatchlist();
        
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