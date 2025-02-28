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
            
            foreach (var bond in bonds)
            {
                await Task.Delay(DelayInMilliseconds);

                var request = CreateGetBondCouponsRequest(bond.InstrumentId);
                var response = await SendGetDividendsRequest(request);

                if (response is null)
                    continue;

                if (response.Events is not null)
                    foreach (var coupon in response.Events)
                        if (coupon is not null)
                        {
                            var bondCoupon = Map(coupon, bond);
                            bondCoupons.Add(bondCoupon);   
                        }
            }

            return bondCoupons;
        }
            
        catch (Exception exception)
        {
            logger.Error(exception);
            return [];
        }
    }
    
    private static BondCoupon Map(Coupon coupon, Bond bond) =>
        new()
        {
            InstrumentId = bond.InstrumentId,
            Ticker = bond.Ticker,
            CouponNumber = coupon.CouponNumber,
            CouponPeriod = coupon.CouponPeriod,
            CouponDate = ConvertHelper.TimestampToDateOnly(coupon.CouponDate),
            CouponStartDate = ConvertHelper.TimestampToDateOnly(coupon.CouponStartDate),
            CouponEndDate = ConvertHelper.TimestampToDateOnly(coupon.CouponEndDate),
            PayOneBond = ConvertHelper.MoneyValueToDouble(coupon.PayOneBond)
        };
    
    private static GetBondCouponsRequest CreateGetBondCouponsRequest(Guid instrumentId) =>
        new()
        {
            InstrumentId = instrumentId.ToString()
        };
    
    private async Task<GetBondCouponsResponse?> SendGetDividendsRequest(GetBondCouponsRequest request)
    {
        try
        {
            return await client.Instruments.GetBondCouponsAsync(request);
        }
        
        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка получения данных. {request}", request);
            return null;
        }
    }
}