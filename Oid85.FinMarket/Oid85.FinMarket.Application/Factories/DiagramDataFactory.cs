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

    private DiagramData<DateOnly, double?> CreateNewDateOnlyDiagramData(List<DateOnly> dates)
    {
        var diagrgamData = new DiagramData<DateOnly, double?>
        {
            Title = "Графики",
            AxisX =
            {
                Title = "Дата",
                Values = dates
            }
        };

        return diagrgamData;
    }

    private async Task<Axis<double?>> CreateClosePricesAxisYByInstrumentIdAsync(
        Guid instrumentId, List<Candle> candles, List<DateOnly> dates)
    {
        var instrument = await instrumentRepository.GetByInstrumentIdAsync(instrumentId);
        var axis = new Axis<double?> { Title = instrument?.Ticker ?? string.Empty };
        
        foreach (var date in dates)
        {
            var candle = candles.FirstOrDefault(x => x.Date == date);

            if (candle == null)
                axis.Values.Add(null);
            else
                axis.Values.Add(candle.Close);
        }

        return axis;
    }

    public async Task<DiagramData<DateOnly, double?>> CreateClosePricesDiagramDataAsync(
        List<Guid> instrumentIds, DateOnly from, DateOnly to)
    {
        var dates = DateHelper.GetDates(from, to);
        var data = await CreateDataDictionaryAsync(instrumentIds, from, to);
        var diagrgamData = CreateNewDateOnlyDiagramData(dates);

        foreach (var instrumentId in instrumentIds)
        {
            var axis = await CreateClosePricesAxisYByInstrumentIdAsync(instrumentId, data[instrumentId], dates);
            diagrgamData.AxisesY.Add(axis);
        }

        return diagrgamData;
    }
}