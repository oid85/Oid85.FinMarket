﻿using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Factories;

public class DiagramDataFactory(
    IInstrumentRepository instrumentRepository,
    IDailyCandleRepository dailyCandleRepository,
    IMultiplicatorRepository multiplicatorRepository) 
    : IDiagramDataFactory
{
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
    
    public async Task<SimpleDiagramData> CreateDailyClosePricesDiagramDataAsync(
        List<Guid> instrumentIds, DateOnly from, DateOnly to)
    {
        var dates = DateHelper.GetDates(from, to);
        var data = await CreateDailyDataDictionaryAsync(instrumentIds, from, to);
        var simpleDiagramData = new SimpleDiagramData { Title = "Графики" };
        
        foreach (var instrumentId in instrumentIds)
        {
            var instrument = await instrumentRepository.GetAsync(instrumentId);
            string ticker = instrument?.Ticker ?? string.Empty;
            string name = instrument?.Name ?? string.Empty;
            
            var dataPointSeries = new SimpleDataPointSeries { Title = $"{name} ({ticker})" };

            foreach (var date in dates)
            {
                var candle = data[instrumentId].FirstOrDefault(x => x.Date == date);
                
                dataPointSeries.Series.Add(candle is null
                    ? new SimpleDataPoint { Date = date.ToString(KnownDateTimeFormats.DateISO), Value = null }
                    : new SimpleDataPoint { Date = date.ToString(KnownDateTimeFormats.DateISO), Value = candle.Close });
            }
            
            simpleDiagramData.Data.Add(dataPointSeries);
        }

        return simpleDiagramData;
    }
    
    public async Task<BubbleDiagramData> CreateMultiplicatorsMCapPeNetDebtEbitdaAsync(List<Guid> instrumentIds)
    {
        var multiplicators = await multiplicatorRepository.GetAsync(instrumentIds);
        var bubbleDiagramData = new BubbleDiagramData { Title = "MCap, P/E, NetDebt/EBITDA" };

        foreach (var multiplicator in multiplicators)
            bubbleDiagramData.Series.Add(
                new()
                {
                    Name = multiplicator.TickerAo,
                    X = multiplicator.MarketCapitalization,
                    Y = multiplicator.Pe,
                    R = multiplicator.NetDebtToEbitda
                });
        
        return bubbleDiagramData;
    }

    public async Task<BacktestResultDiagramData> CreateBacktestResultDiagramDataAsync(Strategy strategy)
    {
        var diagramData = new BacktestResultDiagramData { Title = $"{strategy.StrategyName}"};

        for (int i = 0; i < strategy.Candles.Count; i++)
        {
            diagramData.Data.Series.Add(new BacktestResultDataPoint
            {
                Date = strategy.Candles[i].DateTime.ToString("dd.MM.yyyy"),
                Price = strategy.Candles[i].Close
            });
        }

        for (int i = 0; i < strategy.Positions.Count; i++)
        {
            if (strategy.Positions[i].IsLong)
                diagramData.Data.Series[strategy.Positions[i].EntryCandleIndex].BuyPrice = strategy.Positions[i].EntryPrice;
            
            else if (strategy.Positions[i].IsShort)
                diagramData.Data.Series[strategy.Positions[i].EntryCandleIndex].SellPrice = strategy.Positions[i].EntryPrice;
        }
        
        return diagramData;
    }
}