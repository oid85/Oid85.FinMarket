using Google.Protobuf.WellKnownTypes;
using Oid85.FinMarket.Configuration.Common;
using Oid85.FinMarket.Models;
using Tinkoff.InvestApi.V1;
using Candle = Oid85.FinMarket.Models.Candle;

namespace Oid85.FinMarket.Storage.WebHost.Helpers;

public class TranslateModelHelper
{
    public GetCandlesRequest DownloadRequestToGetCandlesRequest(DownloadRequest downloadRequest)
    {
        var getCandlesRequest = new GetCandlesRequest
        {
            From = Timestamp.FromDateTime(downloadRequest.From),
            To = Timestamp.FromDateTime(downloadRequest.To),
            InstrumentId = downloadRequest.Figi,
            Interval = TimeframeToCandleInterval(downloadRequest.Timeframe)
        };
        
        return getCandlesRequest;
    }

    public CandleInterval TimeframeToCandleInterval(string timeframeName)
    {
        if (timeframeName == TimeframeNames.M1)
            return CandleInterval._1Min;
               
        if (timeframeName == TimeframeNames.H)
            return CandleInterval.Hour;
        
        if (timeframeName == TimeframeNames.D)
            return CandleInterval.Day;

        return CandleInterval.Unspecified;
    }
    
    public string TimeframeToTableName(string timeframeName)
    {
        if (timeframeName == TimeframeNames.M1)
            return TableNames.M1;
               
        if (timeframeName == TimeframeNames.H)
            return TableNames.H;
        
        if (timeframeName == TimeframeNames.D)
            return TableNames.D;

        return String.Empty;
    }
    
    public Candle HistoricCandleToCandle(HistoricCandle historicCandle)
    {
        var candle = new Candle
        {
            Open = QuotationToDouble(historicCandle.Open),
            Close = QuotationToDouble(historicCandle.Close),
            High = QuotationToDouble(historicCandle.High),
            Low = QuotationToDouble(historicCandle.Low),
            Volume = historicCandle.Volume
        };

        return candle;
    }

    public double QuotationToDouble(Quotation quotation)
    {
        double result = (double) quotation.Units + (double) quotation.Nano / 1_000_000_000;
        
        return result;
    }
}