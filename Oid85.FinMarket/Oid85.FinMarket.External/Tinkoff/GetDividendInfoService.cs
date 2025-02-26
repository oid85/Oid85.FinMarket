using NLog;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Domain.Models;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Share = Oid85.FinMarket.Domain.Models.Share;

namespace Oid85.FinMarket.External.Tinkoff;

public class GetDividendInfoService(
    ILogger logger,
    InvestApiClient client)
{
    private const int DelayInMilliseconds = 50;
    
    public async Task<List<DividendInfo>> GetDividendInfoAsync(
    List<Share> shares)
    {
        try
        {
            await Task.Delay(DelayInMilliseconds);
            
            var dividendInfos = new List<DividendInfo>();
            
            var from = DateTime.SpecifyKind(
                new DateTime(DateTime.UtcNow.Year, 1, 1), 
                DateTimeKind.Utc);
            
            var to = from.AddYears(2);

            foreach (var share in shares)
            {
                await Task.Delay(DelayInMilliseconds);
                
                var request = new GetDividendsRequest
                {
                    InstrumentId = share.InstrumentId.ToString()
                };

                var response = await client
                    .Instruments
                    .GetDividendsAsync(request);

                if (response is null)
                    continue;

                var dividends = response.Dividends.ToList();

                if (dividends.Any())
                {
                    foreach (var dividend in dividends)
                    {
                        if (dividend is null)
                            continue;

                        var dividendInfo = new DividendInfo
                        {
                            Ticker = share.Ticker,
                            InstrumentId = share.InstrumentId,
                            DeclaredDate = ConvertHelper.TimestampToDateOnly(dividend.DeclaredDate),
                            RecordDate = ConvertHelper.TimestampToDateOnly(dividend.RecordDate),
                            Dividend = Math.Round(ConvertHelper.MoneyValueToDouble(dividend.DividendNet), 2),
                            DividendPrc = Math.Round(ConvertHelper.QuotationToDouble(dividend.YieldValue), 2)
                        };

                        dividendInfos.Add(dividendInfo);
                    }                    
                }
            }

            return dividendInfos;
        }
            
        catch (Exception exception)
        {
            logger.Error(exception);
            return [];
        }
    }
}