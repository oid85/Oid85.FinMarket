using Oid85.FinMarket.Models;
using Tinkoff.InvestApi.V1;
using Candle = Oid85.FinMarket.Models.Candle;

namespace Oid85.FinMarket.Storage.WebHost.Helpers;

public class TranslateModelHelper
{
    public GetCandlesRequest DownloadRequestToGetCandlesRequest(DownloadRequest downloadRequest)
    {
        var getCandlesRequest = new GetCandlesRequest();


        return getCandlesRequest;
    }
    
    public Candle HistoricCandleToCandle(HistoricCandle historicCandle)
    {
        var candle = new Candle();


        return candle;
    }
}