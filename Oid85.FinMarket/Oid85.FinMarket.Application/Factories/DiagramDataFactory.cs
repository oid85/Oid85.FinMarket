using Oid85.FinMarket.Application.Factories.Builders;
using Oid85.FinMarket.Application.Interfaces.Factories;
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
    IShareMultiplicatorRepository shareMultiplicatorRepository,
    IBankMultiplicatorRepository bankMultiplicatorRepository,
    IRegressionTailRepository regressionTailRepository) 
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
    
    public async Task<BubbleDiagramData> CreateShareMultiplicatorsMCapPeNetDebtEbitdaAsync(List<Guid> instrumentIds)
    {
        var multiplicators = await shareMultiplicatorRepository.GetAsync(instrumentIds);
        var diagramData = new BubbleDiagramData { Title = "MCap, P/E, NetDebt/EBITDA" };

        foreach (var multiplicator in multiplicators)
        {
            diagramData.Series.Add(new BubbleDataPoint
            {
                Name = multiplicator.Ticker,
                X = multiplicator.Pe,
                Y = multiplicator.NetDebtEbitda,
                R = multiplicator.MarketCap
            });
        }
        
        return diagramData;
    }

    public async Task<BubbleDiagramData> CreateBankMultiplicatorsMCapPePbAsync(List<Guid> instrumentIds)
    {
        var multiplicators = await bankMultiplicatorRepository.GetAsync(instrumentIds);
        var diagramData = new BubbleDiagramData { Title = "MCap, P/E, P/B" };
        
        foreach (var multiplicator in multiplicators)
        {
            diagramData.Series.Add(new BubbleDataPoint
            {
                Name = multiplicator.Ticker,
                X = multiplicator.Pe,
                Y = multiplicator.Pb,
                R = multiplicator.MarketCap
            });
        }
        
        return diagramData;
    }
    
    public async Task<BacktestResultDiagramData> CreateBacktestResultDiagramDataAsync(Strategy strategy)
    {
        var diagramData = new BacktestResultDiagramData
        {
            Title = $"{strategy.StrategyName} {strategy.Candles.First().DateTime.ToString(KnownDateTimeFormats.DateISO)} - {strategy.Candles.Last().DateTime.ToString(KnownDateTimeFormats.DateISO)}"
        };

        diagramData = BacktestResultDiagramDataBuilder.SetDates(diagramData, strategy);
        diagramData = BacktestResultDiagramDataBuilder.SetPrices(diagramData, strategy);
        diagramData = BacktestResultDiagramDataBuilder.SetFilters(diagramData, strategy);
        diagramData = BacktestResultDiagramDataBuilder.SetIndicators(diagramData, strategy);
        diagramData = BacktestResultDiagramDataBuilder.SetChannelBands(diagramData, strategy);
        diagramData = BacktestResultDiagramDataBuilder.SetLongPositions(diagramData, strategy);
        diagramData = BacktestResultDiagramDataBuilder.SetShortPositions(diagramData, strategy);
        diagramData = BacktestResultDiagramDataBuilder.SetEquity(diagramData, strategy);
        diagramData = BacktestResultDiagramDataBuilder.SetDrawdown(diagramData, strategy);
        
        return diagramData;
    }

    public async Task<PairArbitrageBacktestResultDiagramData> CreatePairArbitrageBacktestResultDiagramDataAsync(PairArbitrageStrategy strategy)
    {
        var diagramData = new PairArbitrageBacktestResultDiagramData
        {
            Title = $"{strategy.StrategyName} {strategy.Spreads.First().Date.ToString(KnownDateTimeFormats.DateISO)} - {strategy.Spreads.Last().Date.ToString(KnownDateTimeFormats.DateISO)}"
        };

        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetDates(diagramData, strategy);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetFirstPrices(diagramData, strategy);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetSecondPrices(diagramData, strategy);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetSpreads(diagramData, strategy);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetLongShortPositions(diagramData, strategy);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetShortLongPositions(diagramData, strategy);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetEquity(diagramData, strategy);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetDrawdown(diagramData, strategy);
        
        return diagramData;
    }
    
    public async Task<BacktestResultDiagramData> CreateBacktestResultDiagramDataAsync(List<Strategy> strategies)
    {
        var diagramData = new BacktestResultDiagramData
        {
            Title = $"Бэктест стратегий {strategies[0].Candles.First().DateTime.ToString(KnownDateTimeFormats.DateISO)} - {strategies[0].Candles.Last().DateTime.ToString(KnownDateTimeFormats.DateISO)}"
        };
        
        diagramData = BacktestResultDiagramDataBuilder.SetDates(diagramData, strategies);
        diagramData = BacktestResultDiagramDataBuilder.SetPrices(diagramData, strategies);
        diagramData = BacktestResultDiagramDataBuilder.SetLongPositions(diagramData, strategies);
        diagramData = BacktestResultDiagramDataBuilder.SetShortPositions(diagramData, strategies);
        diagramData = BacktestResultDiagramDataBuilder.SetEquity(diagramData, strategies);
        diagramData = BacktestResultDiagramDataBuilder.SetDrawdown(diagramData, strategies);
        
        return diagramData;
    }

    public async Task<PairArbitrageBacktestResultDiagramData> CreatePairArbitrageBacktestResultDiagramDataAsync(List<PairArbitrageStrategy> strategies)
    {
        var diagramData = new PairArbitrageBacktestResultDiagramData
        {
            Title = $"Бэктест стратегий {strategies[0].Spreads.First().Date.ToString(KnownDateTimeFormats.DateISO)} - {strategies[0].Spreads.Last().Date.ToString(KnownDateTimeFormats.DateISO)}"
        };
        
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetDates(diagramData, strategies);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetFirstPrices(diagramData, strategies);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetSecondPrices(diagramData, strategies);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetSpreads(diagramData, strategies);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetLongShortPositions(diagramData, strategies);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetShortLongPositions(diagramData, strategies);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetEquity(diagramData, strategies);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetDrawdown(diagramData, strategies);
        
        return diagramData;
    }    
    
    public async Task<BacktestResultDiagramData> CreateBacktestResultWithoutPriceDiagramDataAsync(List<Strategy> strategies)
    {
        var diagramData = new BacktestResultDiagramData
        {
            Title = $"Бэктест портфеля стратегий"
        };
        
        diagramData = BacktestResultDiagramDataBuilder.SetDates(diagramData, strategies);
        diagramData = BacktestResultDiagramDataBuilder.SetEquity(diagramData, strategies);
        diagramData = BacktestResultDiagramDataBuilder.SetDrawdown(diagramData, strategies);
        
        return diagramData;
    }

    public async Task<PairArbitrageBacktestResultDiagramData> CreatePairArbitrageBacktestResultWithoutPriceDiagramDataAsync(List<PairArbitrageStrategy> strategies)
    {
        var diagramData = new PairArbitrageBacktestResultDiagramData
        {
            Title = $"Бэктест портфеля стратегий"
        };
        
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetDates(diagramData, strategies);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetEquity(diagramData, strategies);
        diagramData = PairArbitrageBacktestResultDiagramDataBuilder.SetDrawdown(diagramData, strategies);
        
        return diagramData;
    }    
    
    public async Task<SimpleDiagramData> CreateSpreadsDiagramDataAsync(DateOnly from, DateOnly to)
    {
        var simpleDiagramData = new SimpleDiagramData { Title = "Спреды" };
        var regressionTails = await regressionTailRepository.GetAllAsync();

        foreach (var tail in regressionTails.OrderBy(x => x.TickerFirst))
        {
            var tailItems = tail.Tails
                .Where(x => x.Date >= from && x.Date <= to)
                .OrderBy(x => x.Date)
                .ToList();

            if (!tailItems.Any())
                continue;
            
            var dataPointSeries = new SimpleDataPointSeries { Title = $"'{tail.TickerFirst}' vs. '{tail.TickerSecond}' {tailItems.First().Date.ToString(KnownDateTimeFormats.DateISO)} - {tailItems.Last().Date.ToString(KnownDateTimeFormats.DateISO)}" };
            
            foreach (var tailItem in tailItems)
                
                dataPointSeries.Series.Add(new SimpleDataPoint
                {
                    Date = tailItem.Date.ToString(KnownDateTimeFormats.DateISO), 
                    Value = Math.Round(tailItem.Value, 2)
                });
            
            if (dataPointSeries.Series.Count > 10)
                simpleDiagramData.Data.Add(dataPointSeries);
        }
        
        return simpleDiagramData;
    }
}