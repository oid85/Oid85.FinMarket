using NLog;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Mapping;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace Oid85.FinMarket.External.Tinkoff;

public class GetForecastService(
    ILogger logger,
    InvestApiClient client)
{
    private const int DelayInMilliseconds = 100;
    
    public async Task<(List<ForecastTarget>, ForecastConsensus)> GetForecastAsync(Guid instrumentId)
    {
        await Task.Delay(DelayInMilliseconds);

        var request = CreateGetForecastRequest(instrumentId);
        var response = await SendGetForecastRequest(request);
        
        if (response is null)
            return ([], new());

        var targets = new List<ForecastTarget>();

        if (response.Targets is not null)
            foreach (var targetItem in response.Targets)
                if (targetItem is not null)
                {
                    var target = TinkoffMapper.Map(targetItem);
                    targets.Add(target);   
                }

        var consensus = response.Consensus is null 
            ? new() 
            : TinkoffMapper.Map(response.Consensus);
        
        return (targets, consensus);
    }
        
    private static GetForecastRequest CreateGetForecastRequest(Guid instrumentId) =>
        new()
        {
            InstrumentId = instrumentId.ToString()
        };
    
    private async Task<GetForecastResponse?> SendGetForecastRequest(GetForecastRequest request)
    {
        try
        {
            return await client.Instruments.GetForecastByAsync(request);
        }
        
        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка получения данных. {request}", request);
            return null;
        }
    }
}