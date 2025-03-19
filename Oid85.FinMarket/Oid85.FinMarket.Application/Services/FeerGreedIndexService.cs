using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Domain.Models;
using Skender.Stock.Indicators;

namespace Oid85.FinMarket.Application.Services;

public class FeerGreedIndexService(
    IInstrumentService instrumentService,
    IInstrumentRepository instrumentRepository,
    IFeerGreedRepository feerGreedRepository,
    ICandleRepository candleRepository) 
    : IFeerGreedIndexService
{
    public async Task ProcessFeerGreedAsync()
    {
        var momentums = await GetMarketMomentumAsync();
        var volatilities = await GetMarketVolatilityAsync();
        var tuple = await GetStockPriceStrengthBreadthAsync();

        // Нормализация значений от 0 до 100
        var normalizedMomentums = Normalize(momentums);
        var normalizedVolatilities = Normalize(volatilities);
        var normalizedStrengths = Normalize(tuple.Strengths);
        var normalizedBreadths = Normalize(tuple.Breadths);
        
        var dates = GetDates();

        var feerGreedIndexes = new List<FearGreedIndex>();

        foreach (var date in dates)
        {
            var momentum = normalizedMomentums[date];
            var volatility = normalizedVolatilities[date];
            var breadth = normalizedStrengths[date];
            var strength = normalizedBreadths[date];
            
            // Для расчета индекса берется среднее значение показателей, нормированных от 0 до 100
            var values = new List<double>();
            
            if (momentum != 0.0) values.Add(momentum);
            if (volatility != 0.0) values.Add(volatility);
            if (breadth != 0.0) values.Add(breadth);
            if (strength != 0.0) values.Add(strength);
            
            var feerGreedIndexValue = values.Count != 0 ? values.Average() : 0.0;

            var feerGreedIndex = new FearGreedIndex
            {
                Date = date,
                MarketMomentum = momentum,
                MarketVolatility = volatility,
                StockPriceBreadth = breadth,
                StockPriceStrength = strength,
                Value = feerGreedIndexValue
            };
            
            feerGreedIndexes.Add(feerGreedIndex);
        }

        await feerGreedRepository.AddAsync(feerGreedIndexes);
    }

    /// <summary>
    /// Моментум — соотношение между индексом Мосбиржи и его 125-дневной средней
    /// </summary>
    private async Task<Dictionary<DateOnly, double>> GetMarketMomentumAsync()
    {
        var indexMoex = await instrumentRepository.GetByTickerAsync("IMOEX");
        var candles = (await candleRepository.GetLastYearAsync(indexMoex!.InstrumentId))
            .Where(x => x.IsComplete).ToList();
        
        return SeriesToAverageRatio(candles, 125);
    }
    
    /// <summary>
    /// Волатильность рынка — соотношение индекса волатильности RVI и его 50-дневной средней
    /// </summary>
    private async Task<Dictionary<DateOnly, double>> GetMarketVolatilityAsync()
    {
        var indexRvi = await instrumentRepository.GetByTickerAsync("RVI");
        var candles = (await candleRepository.GetLastYearAsync(indexRvi!.InstrumentId))
            .Where(x => x.IsComplete).ToList();

        return SeriesToAverageRatio(candles, 50);
    }
    
    /// <summary>
    /// Сила — разница между числом акций, достигших максимумов и минимумов за 52 недели
    /// Ширина — соотношение объемов в растущих и падающих акциях
    /// </summary>
    private async Task<(Dictionary<DateOnly, double> Strengths, Dictionary<DateOnly, double> Breadths)> GetStockPriceStrengthBreadthAsync()
    {
        var strengths = CreateDictionaryWithDates();
        var breadths = CreateDictionaryWithDates();
        
        var data = await CreateDataDictionaryAsync();
        
        var dates = GetDates();
        
        foreach (var date in dates)
        {
            var from = date.AddYears(-1);
            var to = date;

            int countGrowing = 0;
            int countFalling = 0;
            long volumeGrowing = 0;
            long volumeFalling = 0;
            
            foreach (var shareSeries in data)
            {
                var candles = shareSeries.Value
                    .Where(x => x.Date >= from && x.Date <= to).ToList();

                if (candles is not [])
                {
                    double lastClosePrice = candles.Last().Close;
                    long volume = candles.Last().Volume;
                    double max = candles.Select(x => x.Close).Max();
                    double min = candles.Select(x => x.Close).Min();

                    if (lastClosePrice >= max * 0.9)
                    {
                        volumeGrowing += volume;
                        countGrowing++;
                    }

                    if (lastClosePrice <= min * 1.1)
                    {
                        volumeFalling += volume;
                        countFalling++;
                    }
                }
            }
            
            if (countGrowing > 0 && countFalling > 0) strengths[date] = (double) countGrowing / (double) countFalling;
            else if (countGrowing == 0 && countFalling == 0) strengths[date] = 0.0;
            else if (countGrowing > 0 && countFalling == 0) strengths[date] = 100.0;
            else if (countGrowing == 0 && countFalling > 0) strengths[date] = 0.0;

            if (volumeGrowing > 0 && volumeFalling > 0) breadths[date] = (double) volumeGrowing / (double) volumeFalling;
            else if (volumeGrowing == 0 && volumeFalling == 0) breadths[date] = 0.0;
            else if (volumeGrowing > 0 && volumeFalling == 0) breadths[date] = 100.0;
            else if (volumeGrowing == 0 && volumeFalling > 0) breadths[date] = 0.0;
        }
        
        return (Strengths: strengths, Breadths: breadths);
    }
    
    private Dictionary<DateOnly, double> SeriesToAverageRatio(List<Candle> candles, int movingAveragePeriod)
    {
        if (candles is [])
            return [];
        
        var quotes = candles.Select(Map).ToList();
        
        var movingAverageResults = quotes.GetSma(movingAveragePeriod).ToList();

        var dictionary = CreateDictionaryWithDates();

        foreach (var item in dictionary)
        {
            var date = item.Key;
            var candle = candles.FirstOrDefault(x => x.Date == date);
            var movingAverage = movingAverageResults.FirstOrDefault(x => x.Date == date.ToDateTime(TimeOnly.MinValue));
            
            if (candle is null)
                continue;
            
            if (movingAverage?.Sma is null)
                continue;
            
            if (movingAverage.Sma.Value == 0.0)
                continue;

            dictionary[date] = candle.Close / movingAverage.Sma.Value;
        }

        return dictionary;        
    }

    private Dictionary<DateOnly, double> CreateDictionaryWithDates()
    {
        var dates = GetDates();
        return dates.ToDictionary(x => x, _ => 0.0);
    }
    
    private List<DateOnly> GetDates()
    {
        var from = DateOnly.FromDateTime(DateTime.Today.AddYears(-1));
        var to = DateOnly.FromDateTime(DateTime.Today);
        return DateHelper.GetDates(from, to);
    }
    
    private async Task<Dictionary<Guid, List<Candle>>> CreateDataDictionaryAsync()
    {
        var shares = await instrumentService.GetSharesInIndexMoex();

        var dictionary = new Dictionary<Guid, List<Candle>>();
        
        foreach (var share in shares)
        {
            var candles = await candleRepository.GetLastTwoYearsAsync(share.InstrumentId);
            dictionary.Add(share.Id, candles);
        }

        return dictionary;
    }

    private Dictionary<DateOnly, double> Normalize(Dictionary<DateOnly, double> dictionary)
    {
        var maxValue = dictionary.Max(x => x.Value);
        var result = new Dictionary<DateOnly, double>();
        
        foreach (var item in dictionary)
        {
            var key = item.Key;
            var value = item.Value / maxValue * 100.0;
            result.Add(key, value);
        }

        return result;
    }

    private Quote Map(Candle candle) =>
        new()
        {
            Open = Convert.ToDecimal(candle.Open),
            Close = Convert.ToDecimal(candle.Close),
            High = Convert.ToDecimal(candle.High),
            Low = Convert.ToDecimal(candle.Low),
            Date = candle.Date.ToDateTime(TimeOnly.MinValue)
        };
}