using Tinkoff.InvestApi.V1;

namespace Oid85.FinMarket.Common.Helpers;

public class ConvertHelper
{
    public static double QuotationToDouble(Quotation quotation)
    {
        return quotation.Units + quotation.Nano / 1_000_000_000.0;
    }
    
    public static double MoneyValueToDouble(MoneyValue moneyValue)
    {
        return moneyValue.Units + moneyValue.Nano / 1_000_000_000.0;
    }    
}