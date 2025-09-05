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
            diagramData.Series.Add(new BubbleDataPoint()
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
            diagramData.Series.Add(new BubbleDataPoint()
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
        var diagramData = new BacktestResultDiagramData { Title = $"{strategy.StrategyName} {strategy.Candles.First().DateTime.ToString(KnownDateTimeFormats.DateISO)} - {strategy.Candles.Last().DateTime.ToString(KnownDateTimeFormats.DateISO)}"};

        // Price, Filter, Indicator, ChannelBands
        for (int i = 0; i < strategy.Candles.Count; i++)
        {
            diagramData.Data.Series.Add(new BacktestResultDataPoint
            {
                Date = strategy.Candles[i].DateTime.ToString("dd.MM.yyyy"),
                Price = strategy.Candles[i].Close,
                Filter = strategy.GraphPoints[i].Filter,
                Indicator = strategy.GraphPoints[i].Indicator,
                ChannelBands = strategy.GraphPoints[i].ChannelBands,
            });
        }

        // BuyPrice, SellPrice
        for (int i = 0; i < strategy.Positions.Count; i++)
        {
            if (strategy.Positions[i].IsLong)
            {
                diagramData.Data.Series[strategy.Positions[i].EntryCandleIndex].BuyPrice = strategy.Positions[i].EntryPrice;
                
                if (!strategy.Positions[i].IsActive)
                    diagramData.Data.Series[strategy.Positions[i].ExitCandleIndex].SellPrice = strategy.Positions[i].ExitPrice;
            }
        }
        
        // Equity, Drawdown
        var from = strategy.Candles.First().DateTime;
        var to = strategy.Candles.Last().DateTime;
        
        var equity = strategy.EqiutyCurve.Expand(from, to);
        var drawdown = strategy.DrawdownCurve.Expand(from, to);
        
        for (int i = 0; i < diagramData.Data.Series.Count; i++)
        {
            var date = Convert.ToDateTime(diagramData.Data.Series[i].Date);
            diagramData.Data.Series[i].Equity = Math.Round(equity[date], 2);
            diagramData.Data.Series[i].Drawdown = Math.Round(-1 * drawdown[date], 2);
        }
        
        return diagramData;
    }

    public async Task<BacktestResultDiagramData> CreateBacktestResultDiagramDataAsync(List<Strategy> strategies)
    {
        var diagramData = new BacktestResultDiagramData { Title = $"Бэктест стратегий {strategies[0].Candles.First().DateTime.ToString(KnownDateTimeFormats.DateISO)} - {strategies[0].Candles.Last().DateTime.ToString(KnownDateTimeFormats.DateISO)}" };
        
        // Price
        for (int i = 0; i < strategies[0].Candles.Count; i++)
        {
            diagramData.Data.Series.Add(new BacktestResultDataPoint
            {
                Date = strategies[0].Candles[i].DateTime.ToString(KnownDateTimeFormats.DateISO),
                Price = strategies[0].Candles[i].Close
            });
        }
        
        // BuyPrice, SellPrice
        for (int i = 0; i < strategies.Count; i++)
        {
            for (int j = 0; j < strategies[i].Positions.Count; j++)
            {
                if (strategies[i].Positions[j].IsLong)
                {
                    diagramData.Data.Series[strategies[i].Positions[j].EntryCandleIndex].BuyPrice = strategies[i].Positions[j].EntryPrice;
                
                    if (!strategies[i].Positions[j].IsActive)
                        diagramData.Data.Series[strategies[i].Positions[j].ExitCandleIndex].SellPrice = strategies[i].Positions[j].ExitPrice;
                }                
            }
        }

        // Equity, Drawdown
        var from = strategies[0].Candles.First().DateTime;
        var to = strategies[0].Candles.Last().DateTime;

        for (int i = 0; i < strategies.Count; i++)
        {
            var equity = strategies[i].EqiutyCurve.Expand(from, to);
            var drawdown = strategies[i].DrawdownCurve.Expand(from, to);

            for (int j = 0; j < diagramData.Data.Series.Count; j++)
            {
                var date = Convert.ToDateTime(diagramData.Data.Series[j].Date);
                diagramData.Data.Series[j].Equity += Math.Round(equity[date], 2);
                diagramData.Data.Series[j].Drawdown += Math.Round(-1 * drawdown[date], 2);
            }
        }
        
        return diagramData;
    }

    public async Task<BacktestResultDiagramData> CreateBacktestResultWithoutPriceDiagramDataAsync(List<Strategy> strategies)
    {
        var diagramData = new BacktestResultDiagramData { Title = $"Бэктест портфеля стратегий" };
        
        // Date
        for (int i = 0; i < strategies[0].Candles.Count; i++)
        {
            diagramData.Data.Series.Add(new BacktestResultDataPoint
            {
                Date = strategies[0].Candles[i].DateTime.ToString(KnownDateTimeFormats.DateISO)
            });
        }

        // Equity, Drawdown
        var from = strategies[0].Candles.First().DateTime;
        var to = strategies[0].Candles.Last().DateTime;

        for (int i = 0; i < strategies.Count; i++)
        {
            var equity = strategies[i].EqiutyCurve.Expand(from, to);
            var drawdown = strategies[i].DrawdownCurve.Expand(from, to);

            for (int j = 0; j < diagramData.Data.Series.Count; j++)
            {
                var date = Convert.ToDateTime(diagramData.Data.Series[j].Date);
                diagramData.Data.Series[j].Equity += Math.Round(equity[date], 2);
                diagramData.Data.Series[j].Drawdown += Math.Round(-1 * drawdown[date], 2);
            }
        }
        
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

    public async Task<BacktestResultDiagramData> CreatePairArbitrageBacktestResultDiagramDataAsync(PairArbitrageStrategy strategy)
    {

    }

    public async Task<BacktestResultDiagramData> CreatePairArbitrageBacktestResultDiagramDataAsync(List<PairArbitrageStrategy> strategies)
    {

    }

    public async Task<BacktestResultDiagramData> CreatePairArbitrageBacktestResultWithoutPriceDiagramDataAsync(List<PairArbitrageStrategy> strategies)
    {

    }
}