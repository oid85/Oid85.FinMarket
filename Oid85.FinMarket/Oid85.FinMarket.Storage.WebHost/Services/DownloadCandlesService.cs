using Oid85.FinMarket.Configuration.Common;
using Oid85.FinMarket.Models;
using Oid85.FinMarket.Storage.WebHost.Converters;
using Oid85.FinMarket.Storage.WebHost.Repositories;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Candle = Oid85.FinMarket.Models.Candle;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.Storage.WebHost.Services;

public class DownloadCandlesService
{
    private readonly InvestApiClient _investApiClient;
    private readonly ModelConverter _modelConverter;
    private readonly AssetRepository _assetRepository;
    private readonly CandleRepository _candleRepository;
    private readonly ILogger _logger;
    
    public DownloadCandlesService(
        InvestApiClient investApiClient, 
        ModelConverter modelConverter,
        AssetRepository assetRepository, 
        CandleRepository candleRepository, 
        ILogger logger)
    {
        _investApiClient = investApiClient;
        _modelConverter = modelConverter;
        _assetRepository = assetRepository;
        _candleRepository = candleRepository;
        _logger = logger;
    }

    public async Task _1D_DownloadCandles()
    {
        int countDays = 100;
        
        var now = DateTime.UtcNow;
        
        string tableName = TableNames.D;
        
        var assets = await _assetRepository.GetAllAssetsAsync();

        var candlesForSave = new List<Candle>();
        
        for (int i = 0; i < assets.Count; i++)
        {
            var lastCandle = await _candleRepository.GetLastCandleAsync(assets[i], tableName);

            DateTime from = now.Date.AddDays(-1 * countDays);
            DateTime to = now.Date;
            
            if (lastCandle != null) 
                from = lastCandle.DateTime;

            var downloadRequest = new DownloadRequest()
            {
                From = from,
                To = to,
                Timeframe = TimeframeNames.D,
                Figi = assets[i].Figi,
                Ticker = assets[i].Ticker
            };

            var candles = await DownloadCandlesAsync(downloadRequest);

            candlesForSave.AddRange(candles);
        }
        
        await _candleRepository.SaveCandlesAsync(candlesForSave, tableName);
    }

    private async Task<List<Candle>> DownloadCandlesAsync(DownloadRequest downloadRequest)
    {
        var getCandlesRequest = _modelConverter.DownloadRequestToGetCandlesRequest(downloadRequest);

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
                var candle = _modelConverter.HistoricCandleToCandle(getCandlesResponse.Candles[i], downloadRequest.Ticker);
                
                candles.Add(candle);
            }
        }

        return candles;
    }
}