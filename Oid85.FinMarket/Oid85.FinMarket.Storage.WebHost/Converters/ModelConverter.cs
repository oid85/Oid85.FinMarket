using Google.Protobuf.WellKnownTypes;
using Oid85.FinMarket.Configuration.Common;
using Oid85.FinMarket.Models;
using Tinkoff.InvestApi.V1;
using Candle = Oid85.FinMarket.Models.Candle;

namespace Oid85.FinMarket.Storage.WebHost.Converters;

public class ModelConverter
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
        if (timeframeName == TimeframeNames.H)
            return CandleInterval.Hour;
        
        if (timeframeName == TimeframeNames.D)
            return CandleInterval.Day;

        return CandleInterval.Unspecified;
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

    public double QuotationToDouble(Quotation quotation)
    {
        double result = quotation.Units + quotation.Nano / 1_000_000_000.0;
        
        return result;
    }
}