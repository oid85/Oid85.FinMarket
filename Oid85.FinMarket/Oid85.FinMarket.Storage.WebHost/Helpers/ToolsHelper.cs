using Google.Protobuf.WellKnownTypes;
using Oid85.FinMarket.Configuration.Common;
using Oid85.FinMarket.Models;
using Tinkoff.InvestApi.V1;
using Candle = Oid85.FinMarket.Models.Candle;

namespace Oid85.FinMarket.Storage.WebHost.Helpers;

public class ToolsHelper
{
    public DateTime GetBeginDateTimeFor(string timeframeName, Candle? lastCandle)
    {
        var result = DateTime.Now;

        if (lastCandle != null)
        {
            result = lastCandle.DateTime;
            return result;
        }
        
        if (timeframeName == TimeframeNames.M1)
        {
            result = DateTime.Now.AddDays(-10);
        }
        
        else if (timeframeName == TimeframeNames.H)
        {
            result = DateTime.Now.AddDays(-30);
        }
        
        else if (timeframeName == TimeframeNames.D)
        {
            result = DateTime.Now.AddDays(-365 * 5);
        }        
        
        return result;
    }

    public DateTime GetEndDateTimeFor(string timeframeName, DateTime beginDateTime)
    {
        var result = DateTime.Now;

        if (timeframeName == TimeframeNames.M1)
        {
            result = result.AddHours(6);
            
            if (beginDateTime.DayOfWeek == DayOfWeek.Friday)
                result = result.AddDays(3);
            
            if (beginDateTime.DayOfWeek == DayOfWeek.Saturday)
                result = result.AddDays(2);
        }
        
        else if (timeframeName == TimeframeNames.H)
        {
            result = result.AddDays(1);
            
            if (beginDateTime.DayOfWeek == DayOfWeek.Friday)
                result = result.AddDays(3);
            
            if (beginDateTime.DayOfWeek == DayOfWeek.Saturday)
                result = result.AddDays(2);
        }
        
        else if (timeframeName == TimeframeNames.D)
        {
            result = result.AddDays(5);
        }
        
        return result;
    }
}