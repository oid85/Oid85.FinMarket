using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Factories;

public class DiagramDataFactory(
    IInstrumentRepository instrumentRepository,
    ICandleRepository candleRepository) 
    : IDiagramDataFactory
{
    private async Task<Dictionary<Guid, List<Candle>>> CreateDataDictionaryAsync(
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

    public async Task<SimpleDiagramData> CreateClosePricesDiagramDataAsync(
        List<Guid> instrumentIds, DateOnly from, DateOnly to)
    {
        var dates = DateHelper.GetDates(from, to);
        var data = await CreateDataDictionaryAsync(instrumentIds, from, to);
        var simpleDiagramData = new SimpleDiagramData
        {
            Title = "Графики"
        };
        
        foreach (var instrumentId in instrumentIds)
        {
            var instrument = await instrumentRepository.GetByInstrumentIdAsync(instrumentId);
            
            var dataPointSeries = new DataPointSeries()
            {
                Title = instrument?.Ticker ?? string.Empty
            };

            foreach (var date in dates)
            {
                var candle = data[instrumentId].FirstOrDefault(x => x.Date == date);

                dataPointSeries.Data.Add(candle is null
                    ? new DataPoint {Date = date, Value = null}
                    : new DataPoint {Date = date, Value = candle.Close});
            }
            
            simpleDiagramData.Data.Add(dataPointSeries);
        }

        return simpleDiagramData;
    }
}