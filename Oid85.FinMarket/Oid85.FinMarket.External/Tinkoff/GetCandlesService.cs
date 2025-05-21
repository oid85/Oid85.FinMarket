using Google.Protobuf.WellKnownTypes;
using NLog;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Mapping;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Candle = Oid85.FinMarket.Domain.Models.Candle;

namespace Oid85.FinMarket.External.Tinkoff;

public class GetCandlesService(
    ILogger logger,
    InvestApiClient client)
{
    private const int DelayInMilliseconds = 100;
    
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
    
    public Task<List<HourlyCandle>> GetHourlyCandlesAsync(
        Guid instrumentId, DateOnly from, DateOnly to) =>
        GetHourlyCandlesAsync(
            instrumentId, 
            ConvertHelper.DateOnlyToTimestamp(from), 
            ConvertHelper.DateOnlyToTimestamp(to));
    
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
        
        var request = CreateGetCandlesRequest(instrumentId, from, to, CandleInterval.Day);
        var response = await SendGetCandlesRequest(request);

        if (response is null)
            return [];

        var candles = response.Candles.Select(TinkoffMapper.MapCandle).ToList();
        candles.ForEach(x => x.InstrumentId = instrumentId);
        return candles;
    }

    private async Task<List<HourlyCandle>> GetHourlyCandlesAsync(
        Guid instrumentId, Timestamp from, Timestamp to)
    {
        await Task.Delay(DelayInMilliseconds);
        
        var request = CreateGetCandlesRequest(instrumentId, from, to, CandleInterval.Hour);
        var response = await SendGetCandlesRequest(request);

        if (response is null)
            return [];

        var candles = response.Candles.Select(TinkoffMapper.MapHourlyCandle).ToList();
        candles.ForEach(x => x.InstrumentId = instrumentId);
        return candles;
    }    
    
    private async Task<List<FiveMinuteCandle>> GetFiveMinuteCandlesAsync(
        Guid instrumentId, Timestamp from, Timestamp to)
    {
        await Task.Delay(DelayInMilliseconds);
        
        var request = CreateGetCandlesRequest(instrumentId, from, to, CandleInterval._5Min);
        var response = await SendGetCandlesRequest(request);

        if (response is null)
            return [];
        
        var candles = response.Candles.Select(TinkoffMapper.MapFiveMinuteCandle).ToList();
        candles.ForEach(x => x.InstrumentId = instrumentId);
        return candles;
    }
    
    private async Task<GetCandlesResponse?> SendGetCandlesRequest(GetCandlesRequest request)
    {
        try
        {
            return await client.MarketData.GetCandlesAsync(request);
        }
        
        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка получения данных. {request}", request);
            return null;
        }
    }

    private static GetCandlesRequest CreateGetCandlesRequest(
        Guid instrumentId, Timestamp from, Timestamp to, CandleInterval interval) =>
        new()
        {
            InstrumentId = instrumentId.ToString(),
            From = from,
            To = to,
            Interval = interval
        };
}