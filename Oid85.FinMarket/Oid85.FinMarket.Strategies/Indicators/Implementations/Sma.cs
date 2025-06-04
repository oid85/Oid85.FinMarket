using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.Strategies.Indicators.Implementations
{
    public class Sma : Indicator
    {
        public Sma(List<double> values, int period)
        {
            DataSeries ds = Init(values);
            DataSeries indicator = SMA.Series(ds, period);
            Values = ToIndicator(indicator);
        }
    }
}
