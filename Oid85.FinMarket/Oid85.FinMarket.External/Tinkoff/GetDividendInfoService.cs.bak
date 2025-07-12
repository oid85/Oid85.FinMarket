using NLog;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Mapping;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Share = Oid85.FinMarket.Domain.Models.Share;

namespace Oid85.FinMarket.External.Tinkoff;

public class GetDividendInfoService(
    ILogger logger,
    InvestApiClient client)
{
    private const int DelayInMilliseconds = 100;
    
    public async Task<List<DividendInfo>> GetDividendInfoAsync(
    List<Share> shares)
    {
        await Task.Delay(DelayInMilliseconds);
        
        var dividendInfos = new List<DividendInfo>();

        foreach (var share in shares)
        {
            await Task.Delay(DelayInMilliseconds);

            var request = CreateGetDividendsRequest(share.InstrumentId);
            var response = await SendGetDividendsRequest(request);

            if (response is null)
                continue;

            if (response.Dividends is not null)
                foreach (var dividend in response.Dividends)
                    if (dividend is not null)
                    {
                        var dividendInfo = TinkoffMapper.Map(dividend, share);
                        dividendInfos.Add(dividendInfo);   
                    }
        }

        return dividendInfos;
    }
    
    private static GetDividendsRequest CreateGetDividendsRequest(Guid instrumentId) =>
        new()
        {
            InstrumentId = instrumentId.ToString(),
            From = ConvertHelper.DateTimeToTimestamp(DateTime.Today),
            To = ConvertHelper.DateTimeToTimestamp(DateTime.Today.AddYears(1))
        };
    
    private async Task<GetDividendsResponse?> SendGetDividendsRequest(GetDividendsRequest request)
    {
        try
        {
            return await client.Instruments.GetDividendsAsync(request);
        }
        
        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка получения данных. {request}", request);
            return null;
        }
    }
}