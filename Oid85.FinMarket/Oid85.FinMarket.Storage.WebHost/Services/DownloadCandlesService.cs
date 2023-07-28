using Oid85.FinMarket.Models;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Candle = Oid85.FinMarket.Models.Candle;

namespace Oid85.FinMarket.Storage.WebHost.Services;

public class DownloadCandlesService
{
    private readonly InvestApiClient _investApiClient;

    public DownloadCandlesService(InvestApiClient investApiClient)
    {
        _investApiClient = investApiClient;
    }

    public async Task<List<Candle>> DownloadCandles(DownloadRequest downloadRequest)
    {
        var candles = new List<Candle>();
        
        var getCandlesRequest = new GetCandlesRequest();
        
        var getCandlesResponse = await _investApiClient.MarketData.GetCandlesAsync(getCandlesRequest);

        return candles;
    }
}