using NLog;
using Oid85.FinMarket.Common.Helpers;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace Oid85.FinMarket.External.Tinkoff;

public class GetPricesService(
    ILogger logger,
    InvestApiClient client)
{
    private const int ChunkSize = 50;
    private const int DelayInMilliseconds = 50;
    
    public async Task<List<double>> GetPricesAsync(List<Guid> instrumentIds)
    {
        var chunks = instrumentIds.Chunk(ChunkSize);

        var result = new List<double>();

        foreach (var chunk in chunks)
        {
            var chunkInstrumentIds = chunk.ToList();
            result.AddRange(await GetPricesByChunkAsync(chunkInstrumentIds));
        }
        
        return result;
    }

    private async Task<List<double>> GetPricesByChunkAsync(List<Guid> chunkInstrumentIds)
    {
        try
        {
            await Task.Delay(DelayInMilliseconds);
            
            var response = await SendGetLastPricesRequest(chunkInstrumentIds);
                
            if (response is null)
                return [];
                
            var result = response.LastPrices
                .Select(x => ConvertHelper.QuotationToDouble(x.Price))
                .ToList();

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception);
            return [];
        }        
    }
    
    private async Task<GetLastPricesResponse?> SendGetLastPricesRequest(List<Guid> instrumentIds)
    {
        var request = new GetLastPricesRequest();
        request.InstrumentId.AddRange(instrumentIds.Select(x => x.ToString()));
        request.LastPriceType = LastPriceType.LastPriceExchange;
        
        try
        {
            return await client.MarketData.GetLastPricesAsync(request);
        }
        
        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка получения данных. {request}", request);
            return null;
        }
    }
}