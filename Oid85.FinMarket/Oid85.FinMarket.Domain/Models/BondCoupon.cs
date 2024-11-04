namespace Oid85.FinMarket.Domain.Models;

public class BondCoupon
{
    public string Ticker { get; set; }
    public DateTime CouponDate { get; set; }
    public long CouponNumber { get; set; }
    public int CouponPeriod { get; set; }
    public DateTime CouponStartDate { get; set; }
    public DateTime CouponEndDate { get; set; }
    public double PayOneBond { get; set; }
}