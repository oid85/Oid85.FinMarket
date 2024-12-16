using Google.Protobuf.WellKnownTypes;
using Tinkoff.InvestApi.V1;

namespace Oid85.FinMarket.Common.Helpers;

public static class ConvertHelper
{
    public static double QuotationToDouble(Quotation quotation) =>
        quotation.Units + quotation.Nano / 1_000_000_000.0;
    
    public static double MoneyValueToDouble(MoneyValue moneyValue) =>
        moneyValue.Units + moneyValue.Nano / 1_000_000_000.0;
    
    public static DateOnly TimestampToDateOnly(Timestamp timestamp) =>
        DateOnly.FromDateTime(timestamp.ToDateTime());
}