using Oid85.FinMarket.Models;
using Oid85.FinMarket.Storage.WebHost.Helpers;
using Tinkoff.InvestApi;
using Candle = Oid85.FinMarket.Models.Candle;

namespace Oid85.FinMarket.Storage.WebHost.Services;

public class DownloadCandlesService
{
    private readonly InvestApiClient _investApiClient;
    private readonly TranslateModelHelper _translateModelHelper;
    public DownloadCandlesService(
        InvestApiClient investApiClient, 
        TranslateModelHelper translateModelHelper)
    {
        _investApiClient = investApiClient;
        _translateModelHelper = translateModelHelper;
    }

    public async Task ProcessAssets()
    {
        
    }
    
    public async Task<List<Candle>> DownloadCandles(DownloadRequest downloadRequest)
    {
        var getCandlesRequest = _translateModelHelper.DownloadRequestToGetCandlesRequest(downloadRequest);
        
        var getCandlesResponse = await _investApiClient.MarketData.GetCandlesAsync(getCandlesRequest);

        var candles = new List<Candle>();

        if (getCandlesResponse != null)
            candles = getCandlesResponse.Candles
            .Select(item => _translateModelHelper.HistoricCandleToCandle(item))
            .ToList();   
        
        return candles;
    }
}