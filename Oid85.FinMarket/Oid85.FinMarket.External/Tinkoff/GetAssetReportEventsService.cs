using NLog;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Mapping;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Bond = Tinkoff.InvestApi.V1.Bond;

namespace Oid85.FinMarket.External.Tinkoff;

public class GetAssetReportEventsService(
    ILogger logger,
    InvestApiClient client)
{
    private const int DelayInMilliseconds = 50;
    
    public async Task<List<AssetReportEvent>> GetAssetReportEventsAsync(List<Guid> instrumentIds)
    {
        await Task.Delay(DelayInMilliseconds);
            
        var assetReportEvents = new List<AssetReportEvent>();
            
        foreach (var instrumentId in instrumentIds)
        {
            await Task.Delay(DelayInMilliseconds);

            var request = CreateGetAssetReportEventsRequest(instrumentId);
            var response = await SendGetAssetReportEventsRequest(request);

            if (response is null)
                continue;

            if (response.Events is not null)
                foreach (var report in response.Events)
                    if (report is not null)
                    {
                        var assetReportEvent = TinkoffMapper.Map(report);
                        assetReportEvents.Add(assetReportEvent);   
                    }
        }

        return assetReportEvents;
    }
    
    private static GetAssetReportsRequest CreateGetAssetReportEventsRequest(Guid instrumentId) =>
        new()
        {
            InstrumentId = instrumentId.ToString(),
            From = ConvertHelper.DateTimeToTimestamp(DateTime.Today),
            To = ConvertHelper.DateTimeToTimestamp(DateTime.Today.AddYears(1))
        };
    
    private async Task<GetAssetReportsResponse?> SendGetAssetReportEventsRequest(GetAssetReportsRequest request)
    {
        try
        {
            return await client.Instruments.GetAssetReportsAsync(request);
        }
        
        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка получения данных. {request}", request);
            return null;
        }
    }
}