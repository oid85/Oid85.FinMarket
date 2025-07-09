namespace Oid85.FinMarket.Common.Helpers;

public static class DateTimeCurveHelper
{
    public static Dictionary<DateTime, double> Expand(this Dictionary<DateTime, double> curve, DateTime from, DateTime to)
    {
        var curDate = from;
        var dates = new List<DateTime>();
        
        while (curDate <= to)
        {
            dates.Add(curDate);
            curDate = curDate.AddDays(1);
        }

        var result = dates.ToDictionary(date => date, _ => 0.0);

        foreach (var curveItem in curve) result[curveItem.Key] = curveItem.Value;

        var keys = result.Keys.ToList();
        
        for (int i = 1; i < keys.Count; i++)
            if (result[keys[i]] == 0.0)
                result[keys[i]] = result[keys[i - 1]];
        
        return result;
    }
}