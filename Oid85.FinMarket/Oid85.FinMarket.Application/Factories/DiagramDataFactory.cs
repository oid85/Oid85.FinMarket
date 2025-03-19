using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Factories;

public class DiagramDataFactory(
    ICandleRepository candleRepository) 
    : IDiagramDataFactory
{
    private static List<DateOnly> GetDates() => 
        DateHelper.GetDates(
            DateOnly.FromDateTime(DateTime.Today.AddYears(-1)), 
            DateOnly.FromDateTime(DateTime.Today));

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
    
    public async Task<DiagramData<DateOnly, double>> CreateClosePricesDiagramDataAsync(
        List<Guid> instrumentIds, DateOnly from, DateOnly to)
    {
        var dates = GetDates();
        var data = await CreateDataDictionaryAsync(instrumentIds, from, to);
        
        var diagrgamData = new DiagramData<DateOnly, double>
        {
            Title = "Графики цен",
            AxisX =
            {
                Title = "Дата",
                Values = dates
            }
        };

        foreach (var instrumentId in instrumentIds)
        {
            foreach (var date in dates)
            {
                
            }
        }

        return diagrgamData;
    }
}