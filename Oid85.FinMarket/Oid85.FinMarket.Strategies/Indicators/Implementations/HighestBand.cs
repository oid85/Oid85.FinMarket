using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.Strategies.Indicators.Implementations
{
    public class HighestBand : Indicator
    {
        public HighestBand(List<double> values, int period)
        {
            DataSeries ds = Init(values);
            DataSeries indicator = Highest.Series(ds, period);
            Values = ToIndicator(indicator);
        }
    }
}
