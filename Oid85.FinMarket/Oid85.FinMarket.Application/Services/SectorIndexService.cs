using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services;

public class SectorIndexService(
    ITickerListUtilService tickerListUtilService,
    ICandleRepository candleRepository) : ISectorIndexService
{
    /// <inheritdoc />
    public Task CalculateOilAndGasSectorIndexDailyCandlesAsync() =>
        CalculateSectorIndexDailyCandlesAsync(KnownTickerLists.SharesSectorsOilAndGas);
    
    private async Task CalculateSectorIndexDailyCandlesAsync(string tickerList)
    {
        var shares = await tickerListUtilService.GetSharesByTickerListAsync(tickerList);
        var instrumentIds = shares.Select(x => x.InstrumentId).ToList();
        var candles = await CreateSectorIndexDailyCandles(instrumentIds);
        await candleRepository.AddOrUpdateAsync(candles);
    }

    private async Task<List<Candle>> CreateSectorIndexDailyCandles(List<Guid> instrumentIds)
    {
        var dictionary = await CreateDailyDataDictionaryAsync(
            instrumentIds, new DateOnly(2022, 1, 1), DateOnly.FromDateTime(DateTime.Today));

        var normalizeDictionary = NormalizeDictionary(dictionary);
    }

    private static Dictionary<Guid, List<Candle>> NormalizeDictionary(Dictionary<Guid, List<Candle>> dictionary)
    {
        for (int i = 0; i < dictionary.Keys.Count; i++)
        {

        }
    }

    private async Task<Dictionary<Guid, List<Candle>>> CreateDailyDataDictionaryAsync(
        List<Guid> instrumentIds, DateOnly from, DateOnly to)
    {
        var dictionary = new Dictionary<Guid, List<Candle>>();

        foreach (var instrumentId in instrumentIds)
        {
            var candles = await candleRepository.GetAsync(instrumentId, from, to);
            dictionary.Add(instrumentId, candles);
        }

        return dictionary;
    }
}