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
            result = lastCandle.DateTime.AddSeconds(30); // Чтобы не "захватить" при чтении свечу, которая уже в БД
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
            result = beginDateTime.AddHours(6);
            
            if (beginDateTime.DayOfWeek == DayOfWeek.Friday)
                result = beginDateTime.AddDays(3);
            
            if (beginDateTime.DayOfWeek == DayOfWeek.Saturday)
                result = beginDateTime.AddDays(2);
        }
        
        else if (timeframeName == TimeframeNames.H)
        {
            result = beginDateTime.AddDays(1);
            
            if (beginDateTime.DayOfWeek == DayOfWeek.Friday)
                result = beginDateTime.AddDays(3);
            
            if (beginDateTime.DayOfWeek == DayOfWeek.Saturday)
                result = beginDateTime.AddDays(2);
        }
        
        else if (timeframeName == TimeframeNames.D)
        {
            result = beginDateTime.AddDays(5);
        }
        
        return result;
    }
}