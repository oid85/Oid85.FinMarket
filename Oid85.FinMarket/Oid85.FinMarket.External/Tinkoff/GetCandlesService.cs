using Google.Protobuf.WellKnownTypes;
using NLog;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Domain.Models;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Candle = Oid85.FinMarket.Domain.Models.Candle;

namespace Oid85.FinMarket.External.Tinkoff;

public class GetCandlesService(
    ILogger logger,
    InvestApiClient client)
{
    private const int DelayInMilliseconds = 50;
    
    public Task<List<Candle>> GetDailyCandlesAsync(
        Guid instrumentId, DateOnly from, DateOnly to) =>
        GetDailyCandlesAsync(
            instrumentId, 
            ConvertHelper.DateOnlyToTimestamp(from), 
            ConvertHelper.DateOnlyToTimestamp(to));
    
    public Task<List<Candle>> GetDailyCandlesAsync(Guid instrumentId, int year) =>
        GetDailyCandlesAsync(
            instrumentId, 
            ConvertHelper.DateOnlyToTimestamp(new DateOnly(year, 1, 1)), 
            ConvertHelper.DateOnlyToTimestamp(new DateOnly(year, 12, 31)));
    
    public Task<List<FiveMinuteCandle>> GetFiveMinuteCandlesAsync(
        Guid instrumentId, DateTime from, DateTime to) =>
        GetFiveMinuteCandlesAsync(
            instrumentId, 
            ConvertHelper.DateTimeToTimestamp(from), 
            ConvertHelper.DateTimeToTimestamp(to));

    private async Task<List<Candle>> GetDailyCandlesAsync(
        Guid instrumentId, Timestamp from, Timestamp to)
    {
        await Task.Delay(DelayInMilliseconds);
        
        var response = await SendCandlesRequest(
            instrumentId, from, to, CandleInterval.Day);

        if (response is null)
            return [];

        return response.Candles.Select(historicCandle => new Candle
            {
                InstrumentId = instrumentId,
                Open = ConvertHelper.QuotationToDouble(historicCandle.Open),
                Close = ConvertHelper.QuotationToDouble(historicCandle.Close),
                High = ConvertHelper.QuotationToDouble(historicCandle.High),
                Low = ConvertHelper.QuotationToDouble(historicCandle.Low),
                Volume = historicCandle.Volume,
                Date = ConvertHelper.TimestampToDateOnly(historicCandle.Time),
                IsComplete = historicCandle.IsComplete
            })
            .ToList();
    }
    
    private async Task<List<FiveMinuteCandle>> GetFiveMinuteCandlesAsync(
        Guid instrumentId, Timestamp from, Timestamp to)
    {
        await Task.Delay(DelayInMilliseconds);
        
        var response = await SendCandlesRequest(
            instrumentId, from, to, CandleInterval._5Min);

        if (response is null)
            return [];
        
        return response.Candles.Select(historicCandle => new FiveMinuteCandle
            {
                InstrumentId = instrumentId,
                Open = ConvertHelper.QuotationToDouble(historicCandle.Open),
                Close = ConvertHelper.QuotationToDouble(historicCandle.Close),
                High = ConvertHelper.QuotationToDouble(historicCandle.High),
                Low = ConvertHelper.QuotationToDouble(historicCandle.Low),
                Volume = historicCandle.Volume,
                Date = ConvertHelper.TimestampToDateOnly(historicCandle.Time),
                Time = ConvertHelper.TimestampToTimeOnly(historicCandle.Time),
                IsComplete = historicCandle.IsComplete
            })
            .ToList();
    }

    private async Task<GetCandlesResponse?> SendCandlesRequest(
        Guid instrumentId, Timestamp from, Timestamp to, CandleInterval interval)
    {
        try
        {
            var request = new GetCandlesRequest
            {
                InstrumentId = instrumentId.ToString(),
                From = from,
                To = to,
                Interval = interval
            };

            var response = await client.MarketData.GetCandlesAsync(request);

            return response;
        }
        
        catch (Exception exception)
        {
            logger.Error(
                exception, 
                "Ошибка получения данных. {instrumentId}, {from}, {to}, {interval}", 
                instrumentId, from, to, interval);
            return null;
        }
    }
}