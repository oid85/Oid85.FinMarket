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
        var getCandlesRequest = new GetCandlesRequest();

        getCandlesRequest.From = Timestamp.FromDateTime(downloadRequest.From.ToUniversalTime());
        getCandlesRequest.To = Timestamp.FromDateTime(downloadRequest.To.ToUniversalTime());
        getCandlesRequest.InstrumentId = downloadRequest.Figi;
        getCandlesRequest.Interval = TimeframeToCandleInterval(downloadRequest.Timeframe);

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
    
    public string TableNameToTimeframe(string tableName)
    {
        if (tableName == TableNames.M1)
            return TimeframeNames.M1;
               
        if (tableName == TableNames.H)
            return TimeframeNames.H;
        
        if (tableName == TableNames.D)
            return TimeframeNames.D;

        return String.Empty;
    }    
    
    public Candle HistoricCandleToCandle(HistoricCandle historicCandle, string ticker)
    {
        var candle = new Candle
        {
            DateTime = historicCandle.Time.ToDateTime(),
            Open = QuotationToDouble(historicCandle.Open),
            Close = QuotationToDouble(historicCandle.Close),
            High = QuotationToDouble(historicCandle.High),
            Low = QuotationToDouble(historicCandle.Low),
            Volume = historicCandle.Volume,
            Ticker = ticker
        };

        return candle;
    }

    public Candle CandleToCandle(Tinkoff.InvestApi.V1.Candle operativeCandle, string ticker)
    {
        var candle = new Candle
        {
            DateTime = operativeCandle.Time.ToDateTime(),
            Open = QuotationToDouble(operativeCandle.Open),
            Close = QuotationToDouble(operativeCandle.Close),
            High = QuotationToDouble(operativeCandle.High),
            Low = QuotationToDouble(operativeCandle.Low),
            Volume = operativeCandle.Volume,
            Ticker = ticker
        };

        return candle;
    }
    
    public double QuotationToDouble(Quotation quotation)
    {
        double result = (double) quotation.Units + (double) quotation.Nano / 1_000_000_000;
        
        return result;
    }
}