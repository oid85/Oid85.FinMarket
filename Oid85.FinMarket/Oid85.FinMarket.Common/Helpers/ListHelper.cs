namespace Oid85.FinMarket.Common.Helpers;

public static class ListHelper
{
    public static List<T?> ShiftRight<T>(List<T> list)
    {
        var result = new List<T?> { default };

        result.AddRange(list.Take(list.Count - 1));
        
        return result;
    }
}