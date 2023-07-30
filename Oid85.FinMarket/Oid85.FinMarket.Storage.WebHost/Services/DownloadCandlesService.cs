using Oid85.FinMarket.Models;
using Oid85.FinMarket.Storage.WebHost.Helpers;
using Oid85.FinMarket.Storage.WebHost.Repositories;
using Tinkoff.InvestApi;
using Candle = Oid85.FinMarket.Models.Candle;

namespace Oid85.FinMarket.Storage.WebHost.Services;

public class DownloadCandlesService
{
    private readonly InvestApiClient _investApiClient;
    private readonly TranslateModelHelper _translateModelHelper;
    private readonly AssetRepository _assetRepository;
    private readonly CandleRepository _candleRepository;
    
    public DownloadCandlesService(
        InvestApiClient investApiClient, 
        TranslateModelHelper translateModelHelper, 
        AssetRepository assetRepository, 
        CandleRepository candleRepository)
    {
        _investApiClient = investApiClient;
        _translateModelHelper = translateModelHelper;
        _assetRepository = assetRepository;
        _candleRepository = candleRepository;
    }

    public async Task ProcessAssets(string timeframeName)
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