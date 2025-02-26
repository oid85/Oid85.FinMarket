using NLog;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Domain.Models;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Bond = Oid85.FinMarket.Domain.Models.Bond;

namespace Oid85.FinMarket.External.Tinkoff;

public class GetBondCouponsService(
    ILogger logger,
    InvestApiClient client)
{
    private const int DelayInMilliseconds = 50;
    
    public async Task<List<BondCoupon>> GetBondCouponsAsync(List<Bond> bonds)
    {
        try
        {
            await Task.Delay(DelayInMilliseconds);
            
            var bondCoupons = new List<BondCoupon>();
            
            var from = DateTime.SpecifyKind(
                new DateTime(DateTime.UtcNow.Year, 1, 1), 
                DateTimeKind.Utc);
            
            var to = from.AddYears(2);

            for (var i = 0; i < bonds.Count; i++)
            {
                await Task.Delay(DelayInMilliseconds);
                
                var request = new GetBondCouponsRequest
                {
                    InstrumentId = bonds[i].InstrumentId.ToString()
                };

                var response = await client.Instruments.GetBondCouponsAsync(request);

                if (response is null)
                    continue;

                var coupons = response.Events.ToList();

                if (coupons.Any())
                {
                    foreach (var coupon in coupons)
                    {
                        if (coupon is null)
                            continue;

                        var bondCoupon = new BondCoupon
                        {
                            InstrumentId = bonds[i].InstrumentId,
                            Ticker = bonds[i].Ticker,
                            CouponNumber = coupon.CouponNumber,
                            CouponPeriod = coupon.CouponPeriod,
                            CouponDate = ConvertHelper.TimestampToDateOnly(coupon.CouponDate),
                            CouponStartDate = ConvertHelper.TimestampToDateOnly(coupon.CouponStartDate),
                            CouponEndDate = ConvertHelper.TimestampToDateOnly(coupon.CouponEndDate),
                            PayOneBond = ConvertHelper.MoneyValueToDouble(coupon.PayOneBond)
                        };

                        bondCoupons.Add(bondCoupon);
                    }
                }

                double percent = ((i + 1) / (double) bonds.Count) * 100;
                logger.Trace($"Загружены купоны для облигации '{bonds[i].Ticker}'. {i + 1} из {bonds.Count}. {percent:N2} % загружено");
            }

            return bondCoupons;
        }
            
        catch (Exception exception)
        {
            logger.Error(exception);
            return [];
        }
    }
}