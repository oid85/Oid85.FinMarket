using Oid85.FinMarket.Models;
using Oid85.FinMarket.Storage.WebHost.Helpers;
using Oid85.FinMarket.Storage.WebHost.Repositories;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Candle = Oid85.FinMarket.Models.Candle;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.Storage.WebHost.Services;

public class DownloadCandlesService
{
    private readonly InvestApiClient _investApiClient;
    private readonly TranslateModelHelper _translateModelHelper;
    private readonly ToolsHelper _toolsHelper;
    private readonly AssetRepository _assetRepository;
    private readonly CandleRepository _candleRepository;
    private readonly ILogger _logger;
    
    public DownloadCandlesService(
        InvestApiClient investApiClient, 
        TranslateModelHelper translateModelHelper, 
        ToolsHelper toolsHelper, 
        AssetRepository assetRepository, 
        CandleRepository candleRepository, 
        ILogger logger)
    {
        _investApiClient = investApiClient;
        _translateModelHelper = translateModelHelper;
        _toolsHelper = toolsHelper;
        _assetRepository = assetRepository;
        _candleRepository = candleRepository;
        _logger = logger;
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
                Timeframe = timeframeName,
                Figi = assets[i].Figi,
                Ticker = assets[i].Ticker
            };

            var candles = await DownloadCandlesAsync(downloadRequest);

            await _candleRepository.SaveCandlesAsync(candles, tableName);
        }
    }

    private async Task<List<Candle>> DownloadCandlesAsync(DownloadRequest downloadRequest)
    {
        var getCandlesRequest = _translateModelHelper.DownloadRequestToGetCandlesRequest(downloadRequest);

        GetCandlesResponse? getCandlesResponse = null;

        try
        {
            getCandlesResponse = await _investApiClient.MarketData.GetCandlesAsync(getCandlesRequest);
        }
        
        catch (Exception exception)
        {
            _logger.Error($"DownloadCandlesAsync Error: {exception}");
        }

        var candles = new List<Candle>();

        if (getCandlesResponse == null)
            return candles;
        
        if (getCandlesResponse.Candles == null)
            return candles;
        
        if (getCandlesResponse.Candles.Count == 0)
            return candles;

        for (int i = 0; i < getCandlesResponse.Candles.Count; i++)
        {
            if (getCandlesResponse.Candles[i].IsComplete)
            {
                var candle = _translateModelHelper.HistoricCandleToCandle(getCandlesResponse.Candles[i], downloadRequest.Ticker);
                
                candles.Add(candle);
            }
        }

        return candles;
    }
}