using Oid85.FinMarket.Configuration.Common;
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
    private readonly ToolsHelper _toolsHelper;
    private readonly AssetRepository _assetRepository;
    private readonly CandleRepository _candleRepository;
    
    public DownloadCandlesService(
        InvestApiClient investApiClient, 
        TranslateModelHelper translateModelHelper, 
        ToolsHelper toolsHelper, 
        AssetRepository assetRepository, 
        CandleRepository candleRepository)
    {
        _investApiClient = investApiClient;
        _translateModelHelper = translateModelHelper;
        _toolsHelper = toolsHelper;
        _assetRepository = assetRepository;
        _candleRepository = candleRepository;
    }

    public async Task ProcessAssets(string timeframeName)
    {
        string tableName = _translateModelHelper.TimeframeToTableName(timeframeName);
        
        var assets = await _assetRepository.GetAllAssetsAsync();
        
        for (int i = 0; i < assets.Count; i++)
        {
            var lastCandle = await _candleRepository.GetLastCandleAsync(assets[i], tableName);

            var beginDateTime = _toolsHelper.GetBeginDateTimeFor(timeframeName, lastCandle);

            var endDateTime = _toolsHelper.GetEndDateTimeFor(timeframeName, beginDateTime);

            var downloadRequest = new DownloadRequest()
            {
                From = beginDateTime,
                To = endDateTime,
                Timeframe = tableName,
                Figi = assets[i].Figi
            };

            var candles = await DownloadCandlesAsync(downloadRequest);

            await _candleRepository.SaveCandlesAsync(candles, tableName);
        }
    }

    private async Task<List<Candle>> DownloadCandlesAsync(DownloadRequest downloadRequest)
    {
        var getCandlesRequest = _translateModelHelper.DownloadRequestToGetCandlesRequest(downloadRequest);
        
        var getCandlesResponse = await _investApiClient.MarketData.GetCandlesAsync(getCandlesRequest);

        var candles = new List<Candle>();

        if (getCandlesResponse != null && getCandlesResponse.Candles != null)
            candles = getCandlesResponse.Candles
            .Select(item => _translateModelHelper.HistoricCandleToCandle(item))
            .ToList();   
        
        return candles;
    }
}