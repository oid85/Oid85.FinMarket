using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services;

public class SectorIndexService(
    ITickerListUtilService tickerListUtilService,
    IDailyCandleRepository dailyCandleRepository) 
    : ISectorIndexService
{
    /// <inheritdoc />
    public Task CalculateOilAndGasSectorIndexDailyCandlesAsync() =>
        CalculateSectorIndexDailyCandlesAsync(
            KnownInstrumentIds.OilAndGasSectorIndex, 
            KnownTickerLists.SharesSectorsOilAndGas);

    public Task CalculateBanksSectorIndexDailyCandlesAsync() =>
        CalculateSectorIndexDailyCandlesAsync(
            KnownInstrumentIds.BanksSectorIndex, 
            KnownTickerLists.SharesSectorsBanks);

    public Task CalculateEnergSectorIndexDailyCandlesAsync() =>
        CalculateSectorIndexDailyCandlesAsync(
            KnownInstrumentIds.EnergSectorIndex, 
            KnownTickerLists.SharesSectorsEnerg);

    public Task CalculateFinanceSectorIndexDailyCandlesAsync() =>
        CalculateSectorIndexDailyCandlesAsync(
            KnownInstrumentIds.FinanceSectorIndex, 
            KnownTickerLists.SharesSectorsFinance);

    public Task CalculateHousingAndUtilitiesSectorIndexDailyCandlesAsync() =>
        CalculateSectorIndexDailyCandlesAsync(
            KnownInstrumentIds.HousingAndUtilitiesSectorIndex, 
            KnownTickerLists.SharesSectorsHousingAndUtilities);

    public Task CalculateIronAndSteelIndustrySectorIndexDailyCandlesAsync() =>
        CalculateSectorIndexDailyCandlesAsync(
            KnownInstrumentIds.IronAndSteelIndustrySectorIndex, 
            KnownTickerLists.SharesSectorsIronAndSteelIndustry);

    public Task CalculateItSectorIndexDailyCandlesAsync() =>
        CalculateSectorIndexDailyCandlesAsync(
            KnownInstrumentIds.ItSectorIndex, 
            KnownTickerLists.SharesSectorsIt);

    public Task CalculateMiningSectorIndexDailyCandlesAsync() =>
        CalculateSectorIndexDailyCandlesAsync(
            KnownInstrumentIds.MiningSectorIndex, 
            KnownTickerLists.SharesSectorsMining);

    public Task CalculateNonFerrousMetallurgySectorIndexDailyCandlesAsync() =>
        CalculateSectorIndexDailyCandlesAsync(
            KnownInstrumentIds.NonFerrousMetallurgySectorIndex, 
            KnownTickerLists.SharesSectorsNonFerrousMetallurgy);

    public Task CalculateRetailSectorIndexDailyCandlesAsync() =>
        CalculateSectorIndexDailyCandlesAsync(
            KnownInstrumentIds.RetailSectorIndex, 
            KnownTickerLists.SharesSectorsRetail);

    public Task CalculateTelecomSectorIndexDailyCandlesAsync() =>
        CalculateSectorIndexDailyCandlesAsync(
            KnownInstrumentIds.TelecomSectorIndex, 
            KnownTickerLists.SharesSectorsTelecom);

    public Task CalculateTransportSectorIndexDailyCandlesAsync() =>
        CalculateSectorIndexDailyCandlesAsync(
            KnownInstrumentIds.TransportSectorIndex, 
            KnownTickerLists.SharesSectorsTransport);

    private async Task CalculateSectorIndexDailyCandlesAsync(Guid instrumentId, string tickerList)
    {
        var shares = await tickerListUtilService.GetSharesByTickerListAsync(tickerList);
        var instrumentIds = shares.Select(x => x.InstrumentId).ToList();
        var candles = await CreateSectorIndexDailyCandles(instrumentId, instrumentIds);
        await dailyCandleRepository.AddOrUpdateAsync(candles);
    }

    private async Task<List<DailyCandle>> CreateSectorIndexDailyCandles(Guid instrumentId, List<Guid> instrumentIds)
    {
        var from = new DateOnly(2022, 1, 1);
        var to = DateOnly.FromDateTime(DateTime.Today);
        var dictionary = await CreateDailyDataDictionaryAsync(instrumentIds, from, to);
        var normalizeDictionary = NormalizeDictionary(dictionary);
        var dates = DateHelper.GetDates(from, to);

        var sectorCandles = new List<DailyCandle>();

        foreach (var date in dates)
        {
            var sectorCandle = new DailyCandle
            {
                InstrumentId = instrumentId,
                Open = 0.0,
                Close = 0.0,
                High = 0.0,
                Low = 0.0,
                Date = date,
                IsComplete = true
            };
            
            foreach (var item in normalizeDictionary)
            {
                var candle = item.Value.FirstOrDefault(x => x.Date == date);

                if (candle is not null)
                {
                    sectorCandle.Open += candle.Open;
                    sectorCandle.Close += candle.Close;
                    sectorCandle.High += candle.High;
                    sectorCandle.Low += candle.Low;
                }
            }
            
            sectorCandles.Add(sectorCandle);
        }

        return sectorCandles;
    }

    private static Dictionary<Guid, List<DailyCandle>> NormalizeDictionary(Dictionary<Guid, List<DailyCandle>> dictionary)
    {
        var result = new Dictionary<Guid, List<DailyCandle>>();

        foreach (var item in dictionary)
        {
            var normalizedCandles = NormalizeCandles(item.Value);
            result.Add(item.Key, normalizedCandles);
        }

        return result;
    }

    private static List<DailyCandle> NormalizeCandles(List<DailyCandle> candles)
    {
        var result = new List<DailyCandle>();

        for (int i = 0; i < candles.Count; i++)
        {
            var normalizedCandle = new DailyCandle()
            {
                InstrumentId = candles[i].InstrumentId,
                Open = candles[i].Open / candles[0].Open,
                Close = candles[i].Close / candles[0].Close,
                High = candles[i].High / candles[0].High,
                Low = candles[i].Low / candles[0].Low,
                Date = candles[i].Date,
                IsComplete = true
            };
            
            result.Add(normalizedCandle);
        }
        
        return result;
    }

    private async Task<Dictionary<Guid, List<DailyCandle>>> CreateDailyDataDictionaryAsync(
        List<Guid> instrumentIds, DateOnly from, DateOnly to)
    {
        var dictionary = new Dictionary<Guid, List<DailyCandle>>();

        foreach (var instrumentId in instrumentIds)
        {
            var candles = await dailyCandleRepository.GetAsync(instrumentId, from, to);
            dictionary.Add(instrumentId, candles);
        }

        return dictionary;
    }
}