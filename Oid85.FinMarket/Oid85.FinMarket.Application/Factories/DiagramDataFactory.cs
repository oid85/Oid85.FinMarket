using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Factories;

public class DiagramDataFactory(
    IInstrumentRepository instrumentRepository,
    ICandleRepository candleRepository,
    IFiveMinuteCandleRepository fiveMinuteCandleRepository) 
    : IDiagramDataFactory
{
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

    private async Task<Dictionary<Guid, List<FiveMinuteCandle>>> CreateFiveMinutesDataDictionaryAsync(
        List<Guid> instrumentIds, DateOnly from, DateOnly to)
    {
        var dictionary = new Dictionary<Guid, List<FiveMinuteCandle>>();

        foreach (var instrumentId in instrumentIds)
        {
            var candles = await fiveMinuteCandleRepository.GetAsync(instrumentId, from, to);
            dictionary.Add(instrumentId, candles);
        }

        return dictionary;
    }    
    
    public async Task<SimpleDiagramData> CreateDailyClosePricesDiagramDataAsync(
        List<Guid> instrumentIds, DateOnly from, DateOnly to)
    {
        var dates = DateHelper.GetDates(from, to);
        var data = await CreateDailyDataDictionaryAsync(instrumentIds, from, to);
        var simpleDiagramData = new SimpleDiagramData { Title = "Графики" };
        
        foreach (var instrumentId in instrumentIds)
        {
            var instrument = await instrumentRepository.GetByInstrumentIdAsync(instrumentId);
            string ticker = instrument?.Ticker ?? string.Empty;
            string name = instrument?.Name ?? string.Empty;
            
            var dataPointSeries = new DataPointSeries { Title = $"{name} ({ticker})" };

            foreach (var date in dates)
            {
                var candle = data[instrumentId].FirstOrDefault(x => x.Date == date);
                
                dataPointSeries.Series.Add(candle is null
                    ? new DataPoint { Date = date.ToString(KnownDateTimeFormats.DateISO), Value = null }
                    : new DataPoint { Date = date.ToString(KnownDateTimeFormats.DateISO), Value = candle.Close });
            }
            
            simpleDiagramData.Data.Add(dataPointSeries);
        }

        return simpleDiagramData;
    }

    public async Task<SimpleDiagramData> CreateFiveMinutesClosePricesDiagramDataAsync(List<Guid> instrumentIds, DateOnly from, DateOnly to)
    {
        var dateTimes = DateHelper.GetFiveMinutesDateTimes(from, to);
        var data = await CreateFiveMinutesDataDictionaryAsync(instrumentIds, from, to);
        var simpleDiagramData = new SimpleDiagramData { Title = "Графики (5 мин)" };
        
        foreach (var instrumentId in instrumentIds)
        {
            var instrument = await instrumentRepository.GetByInstrumentIdAsync(instrumentId);
            string ticker = instrument?.Ticker ?? string.Empty;
            string name = instrument?.Name ?? string.Empty;
            
            var dataPointSeries = new DataPointSeries { Title = $"{name} ({ticker})" };

            foreach (var dateTime in dateTimes)
            {
                var candle = data[instrumentId].FirstOrDefault(
                    x => 
                        x.Date == DateOnly.FromDateTime(dateTime) &&
                        x.Time == TimeOnly.FromDateTime(dateTime));
                
                dataPointSeries.Series.Add(candle is null
                    ? new DataPoint { Date = dateTime.ToString(KnownDateTimeFormats.DateTimeISO), Value = null }
                    : new DataPoint { Date = dateTime.ToString(KnownDateTimeFormats.DateTimeISO), Value = candle.Close });
            }
            
            simpleDiagramData.Data.Add(dataPointSeries);
        }

        return simpleDiagramData;
    }
}