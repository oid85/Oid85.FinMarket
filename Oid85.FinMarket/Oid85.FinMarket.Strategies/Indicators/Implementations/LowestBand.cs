using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.Strategies.Indicators.Implementations
{
    public class LowestBand : Indicator
    {
        public LowestBand(List<double> values, int period)
        {
            DataSeries ds = Init(values);
            DataSeries indicator = Lowest.Series(ds, period);
            Values = ToIndicator(indicator);
        }
    }
}
