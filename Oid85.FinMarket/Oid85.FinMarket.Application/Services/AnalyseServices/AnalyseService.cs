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
    YieldLtmAnalyseService yieldLtmAnalyseService,
    AtrAnalyseService atrAnalyseService,
    DonchianAnalyseService donchianAnalyseService)
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
            await atrAnalyseService.AtrAnalyseAsync(instrument.InstrumentId);
            await donchianAnalyseService.DonchianAnalyseAsync(instrument.InstrumentId);
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
            await atrAnalyseService.AtrAnalyseAsync(instrument.InstrumentId);
            await donchianAnalyseService.DonchianAnalyseAsync(instrument.InstrumentId);			
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
            await atrAnalyseService.AtrAnalyseAsync(instrument.InstrumentId);
            await donchianAnalyseService.DonchianAnalyseAsync(instrument.InstrumentId);
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
            await atrAnalyseService.AtrAnalyseAsync(instrument.InstrumentId);
            await donchianAnalyseService.DonchianAnalyseAsync(instrument.InstrumentId);			
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
            await atrAnalyseService.AtrAnalyseAsync(instrument.InstrumentId);
            await donchianAnalyseService.DonchianAnalyseAsync(instrument.InstrumentId);			
        }
            
        return true;
    }

    public async Task<bool> DailyAnalyseOilAndGasSectorIndexAsync()
    {
        // Вызов методов анализа
        await supertrendAnalyseService.SupertrendAnalyseAsync(KnownInstrumentIds.OilAndGasSectorIndex);
        await yieldLtmAnalyseService.YieldLtmAnalyseAsync(KnownInstrumentIds.OilAndGasSectorIndex);
        await drawdownFromMaximumAnalyseService.DrawdownFromMaximumAnalyseAsync(KnownInstrumentIds.OilAndGasSectorIndex);
        await atrAnalyseService.AtrAnalyseAsync(KnownInstrumentIds.OilAndGasSectorIndex);
        await donchianAnalyseService.DonchianAnalyseAsync(KnownInstrumentIds.OilAndGasSectorIndex);
        
        return true;
    }

    public async Task<bool> DailyAnalyseBanksSectorIndexAsync()
    {
        // Вызов методов анализа
        await supertrendAnalyseService.SupertrendAnalyseAsync(KnownInstrumentIds.BanksSectorIndex);
        await yieldLtmAnalyseService.YieldLtmAnalyseAsync(KnownInstrumentIds.BanksSectorIndex);
        await drawdownFromMaximumAnalyseService.DrawdownFromMaximumAnalyseAsync(KnownInstrumentIds.BanksSectorIndex);
        await atrAnalyseService.AtrAnalyseAsync(KnownInstrumentIds.BanksSectorIndex);
        await donchianAnalyseService.DonchianAnalyseAsync(KnownInstrumentIds.BanksSectorIndex);
        
        return true;
    }

    public async Task<bool> DailyAnalyseEnergSectorIndexAsync()
    {
        // Вызов методов анализа
        await supertrendAnalyseService.SupertrendAnalyseAsync(KnownInstrumentIds.EnergSectorIndex);
        await yieldLtmAnalyseService.YieldLtmAnalyseAsync(KnownInstrumentIds.EnergSectorIndex);
        await drawdownFromMaximumAnalyseService.DrawdownFromMaximumAnalyseAsync(KnownInstrumentIds.EnergSectorIndex);
        await atrAnalyseService.AtrAnalyseAsync(KnownInstrumentIds.EnergSectorIndex);
        await donchianAnalyseService.DonchianAnalyseAsync(KnownInstrumentIds.EnergSectorIndex);
        
        return true;
    }

    public async Task<bool> DailyAnalyseFinanceSectorIndexAsync()
    {
        // Вызов методов анализа
        await supertrendAnalyseService.SupertrendAnalyseAsync(KnownInstrumentIds.FinanceSectorIndex);
        await yieldLtmAnalyseService.YieldLtmAnalyseAsync(KnownInstrumentIds.FinanceSectorIndex);
        await drawdownFromMaximumAnalyseService.DrawdownFromMaximumAnalyseAsync(KnownInstrumentIds.FinanceSectorIndex);
        await atrAnalyseService.AtrAnalyseAsync(KnownInstrumentIds.FinanceSectorIndex);
        await donchianAnalyseService.DonchianAnalyseAsync(KnownInstrumentIds.FinanceSectorIndex);
        
        return true;
    }

    public async Task<bool> DailyAnalyseHousingAndUtilitiesSectorIndexAsync()
    {
        // Вызов методов анализа
        await supertrendAnalyseService.SupertrendAnalyseAsync(KnownInstrumentIds.HousingAndUtilitiesSectorIndex);
        await yieldLtmAnalyseService.YieldLtmAnalyseAsync(KnownInstrumentIds.HousingAndUtilitiesSectorIndex);
        await drawdownFromMaximumAnalyseService.DrawdownFromMaximumAnalyseAsync(KnownInstrumentIds.HousingAndUtilitiesSectorIndex);
        await atrAnalyseService.AtrAnalyseAsync(KnownInstrumentIds.HousingAndUtilitiesSectorIndex);
        await donchianAnalyseService.DonchianAnalyseAsync(KnownInstrumentIds.HousingAndUtilitiesSectorIndex);
        
        return true;
    }

    public async Task<bool> DailyAnalyseIronAndSteelIndustrySectorIndexAsync()
    {
        // Вызов методов анализа
        await supertrendAnalyseService.SupertrendAnalyseAsync(KnownInstrumentIds.IronAndSteelIndustrySectorIndex);
        await yieldLtmAnalyseService.YieldLtmAnalyseAsync(KnownInstrumentIds.IronAndSteelIndustrySectorIndex);
        await drawdownFromMaximumAnalyseService.DrawdownFromMaximumAnalyseAsync(KnownInstrumentIds.IronAndSteelIndustrySectorIndex);
        await atrAnalyseService.AtrAnalyseAsync(KnownInstrumentIds.IronAndSteelIndustrySectorIndex);
        await donchianAnalyseService.DonchianAnalyseAsync(KnownInstrumentIds.IronAndSteelIndustrySectorIndex);
        
        return true;
    }

    public async Task<bool> DailyAnalyseItSectorIndexAsync()
    {
        // Вызов методов анализа
        await supertrendAnalyseService.SupertrendAnalyseAsync(KnownInstrumentIds.ItSectorIndex);
        await yieldLtmAnalyseService.YieldLtmAnalyseAsync(KnownInstrumentIds.ItSectorIndex);
        await drawdownFromMaximumAnalyseService.DrawdownFromMaximumAnalyseAsync(KnownInstrumentIds.ItSectorIndex);
        await atrAnalyseService.AtrAnalyseAsync(KnownInstrumentIds.ItSectorIndex);
        await donchianAnalyseService.DonchianAnalyseAsync(KnownInstrumentIds.ItSectorIndex);
        
        return true;
    }

    public async Task<bool> DailyAnalyseMiningSectorIndexAsync()
    {
        // Вызов методов анализа
        await supertrendAnalyseService.SupertrendAnalyseAsync(KnownInstrumentIds.MiningSectorIndex);
        await yieldLtmAnalyseService.YieldLtmAnalyseAsync(KnownInstrumentIds.MiningSectorIndex);
        await drawdownFromMaximumAnalyseService.DrawdownFromMaximumAnalyseAsync(KnownInstrumentIds.MiningSectorIndex);
        await atrAnalyseService.AtrAnalyseAsync(KnownInstrumentIds.MiningSectorIndex);
        await donchianAnalyseService.DonchianAnalyseAsync(KnownInstrumentIds.MiningSectorIndex);
        
        return true;
    }

    public async Task<bool> DailyAnalyseNonFerrousMetallurgySectorIndexAsync()
    {
        // Вызов методов анализа
        await supertrendAnalyseService.SupertrendAnalyseAsync(KnownInstrumentIds.NonFerrousMetallurgySectorIndex);
        await yieldLtmAnalyseService.YieldLtmAnalyseAsync(KnownInstrumentIds.NonFerrousMetallurgySectorIndex);
        await drawdownFromMaximumAnalyseService.DrawdownFromMaximumAnalyseAsync(KnownInstrumentIds.NonFerrousMetallurgySectorIndex);
        await atrAnalyseService.AtrAnalyseAsync(KnownInstrumentIds.NonFerrousMetallurgySectorIndex);
        await donchianAnalyseService.DonchianAnalyseAsync(KnownInstrumentIds.NonFerrousMetallurgySectorIndex);
        
        return true;
    }

    public async Task<bool> DailyAnalyseRetailSectorIndexAsync()
    {
        // Вызов методов анализа
        await supertrendAnalyseService.SupertrendAnalyseAsync(KnownInstrumentIds.RetailSectorIndex);
        await yieldLtmAnalyseService.YieldLtmAnalyseAsync(KnownInstrumentIds.RetailSectorIndex);
        await drawdownFromMaximumAnalyseService.DrawdownFromMaximumAnalyseAsync(KnownInstrumentIds.RetailSectorIndex);
        await atrAnalyseService.AtrAnalyseAsync(KnownInstrumentIds.RetailSectorIndex);
        await donchianAnalyseService.DonchianAnalyseAsync(KnownInstrumentIds.RetailSectorIndex);
        
        return true;
    }

    public async Task<bool> DailyAnalyseTelecomSectorIndexAsync()
    {
        // Вызов методов анализа
        await supertrendAnalyseService.SupertrendAnalyseAsync(KnownInstrumentIds.TelecomSectorIndex);
        await yieldLtmAnalyseService.YieldLtmAnalyseAsync(KnownInstrumentIds.TelecomSectorIndex);
        await drawdownFromMaximumAnalyseService.DrawdownFromMaximumAnalyseAsync(KnownInstrumentIds.TelecomSectorIndex);
        await atrAnalyseService.AtrAnalyseAsync(KnownInstrumentIds.TelecomSectorIndex);
        await donchianAnalyseService.DonchianAnalyseAsync(KnownInstrumentIds.TelecomSectorIndex);
        
        return true;
    }
}