using Google.Protobuf.WellKnownTypes;
using Tinkoff.InvestApi.V1;

namespace Oid85.FinMarket.Common.Helpers;

public static class ConvertHelper
{
    public static double QuotationToDouble(Quotation quotation)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (quotation is null)
            return 0.0;
        
        return quotation.Units + quotation.Nano / 1_000_000_000.0;
    }

    public static double MoneyValueToDouble(MoneyValue moneyValue)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (moneyValue is null)
            return 0.0;
        
        return moneyValue.Units + moneyValue.Nano / 1_000_000_000.0;
    }

    public static Timestamp DateOnlyToTimestamp(DateOnly dateOnly) => 
        Timestamp.FromDateTime(dateOnly.ToDateTime(TimeOnly.MinValue).ToUniversalTime());

    public static Timestamp DateTimeToTimestamp(DateTime dateOnly) => 
        Timestamp.FromDateTime(dateOnly.ToUniversalTime());
    
    public static DateOnly TimestampToDateOnly(Timestamp timestamp)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (timestamp is null)
            return DateOnly.MinValue;
            
        return DateOnly.FromDateTime(timestamp.ToDateTime());
    }
    
    public static TimeOnly TimestampToTimeOnly(Timestamp timestamp)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (timestamp is null)
            return TimeOnly.MinValue;
            
        return TimeOnly.FromDateTime(timestamp.ToDateTime());
    }
    
    public static DateTime TimestampToDateTime(Timestamp timestamp)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (timestamp is null)
            return DateTime.MinValue;

        return timestamp.ToDateTime();
    }    
    
    public static string Base64Encode(string text) => 
        Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(text));

    public static string Base64Decode(string base64) => 
        System.Text.Encoding.UTF8.GetString(
            Convert.FromBase64String(base64));
    
    /// <summary>
    /// Преобразует HSL в RGB
    /// </summary>
    /// <param name="h">Тон, должен быть в [0, 360]</param>
    /// <param name="s">Насыщенность, должна быть в [0, 1]</param>
    /// <param name="l">Яркость, должна быть в [0, 1]</param>
    public static (int r, int g, int b) HsLtoRgb(double h, double s, double l)
    {
        if (s == 0)
            return (Convert.ToInt32(l * 255.0), Convert.ToInt32(l * 255.0), Convert.ToInt32(l * 255.0));

        double q = l < 0.5 
            ? l * (1.0 + s) 
            : l + s - l * s;
            
        double p = 2.0 * l - q;

        double hk = h / 360.0;
        double[] t = new double[3];
            
        t[0] = hk + 1.0 / 3.0;
        t[1] = hk;
        t[2] = hk - 1.0 / 3.0;

        for (int i = 0; i < 3; i++)
        {
            if (t[i] < 0) 
                t[i] += 1.0;
                
            if (t[i] > 1) 
                t[i] -= 1.0;

            if (t[i] * 6 < 1)
                t[i] = p + (q - p) * 6.0 * t[i];
                
            else if (t[i] * 2.0 < 1)
                t[i] = q;
                
            else if (t[i] * 3.0 < 2)
                t[i] = p + (q - p) * (2.0 / 3.0 - t[i]) * 6.0;
                
            else t[i] = p;
        }

        return (Convert.ToInt32(t[0] * 255.0), Convert.ToInt32(t[1] * 255.0), Convert.ToInt32(t[2] * 255.0));
    }
    
    /// <summary>
    /// преобразует формат цвета RGB в шестнадцатеричный цвет.
    /// </summary>
    /// <param name="r">значение красного.</param>
    /// <param name="g">значение зеленого.</param>
    /// <param name="b">значение синего.</param>
    public static string RgbToHex(int r, int g, int b) => 
        $"#{r:x2}{g:x2}{b:x2}".ToUpper();
}