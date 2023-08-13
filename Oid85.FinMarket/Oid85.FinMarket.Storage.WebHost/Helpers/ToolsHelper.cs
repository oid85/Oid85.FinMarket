using Oid85.FinMarket.Configuration.Common;
using Candle = Oid85.FinMarket.Models.Candle;

namespace Oid85.FinMarket.Storage.WebHost.Helpers;

public class ToolsHelper
{
    public DateTime GetBeginDateTimeFor(string timeframeName, Candle? lastCandle)
    {
        var result = DateTime.UtcNow;

        if (timeframeName == TimeframeNames.H)
        {
            if (lastCandle != null)
            {
                result = lastCandle.DateTime.AddHours(1);
                return result;
            }
            
            result = DateTime.UtcNow.AddMonths(-1);
        }
        
        else if (timeframeName == TimeframeNames.D)
        {
            if (lastCandle != null)
            {
                result = lastCandle.DateTime.AddDays(1);
                return result;
            }
            
            result = DateTime.UtcNow.AddYears(-5);
        }        
        
        return result;
    }

    public DateTime GetEndDateTimeFor(string timeframeName, DateTime beginDateTime)
    {
        var result = DateTime.UtcNow;

        if (timeframeName == TimeframeNames.H)
        {
            result = beginDateTime.AddHours(100);
            
            if (beginDateTime.DayOfWeek == DayOfWeek.Friday)
                result = beginDateTime.AddDays(3);
            
            if (beginDateTime.DayOfWeek == DayOfWeek.Saturday)
                result = beginDateTime.AddDays(2);
        }
        
        else if (timeframeName == TimeframeNames.D)
        {
            result = beginDateTime.AddDays(100);
        }
        
        return result;
    }
}