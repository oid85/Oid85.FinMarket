using WealthLab;

namespace Oid85.FinMarket.Strategies.Indicators.Implementations
{
    public class Er : Indicator
    {
        public Er(List<double> values, int period)
        {
            DataSeries ds = Init(values);
            DataSeries indicator = Community.Indicators.ER.Series(ds, period);
            Values = ToIndicator(indicator);
        }
    }
}
