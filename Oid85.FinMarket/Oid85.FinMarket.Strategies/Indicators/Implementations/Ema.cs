using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.Strategies.Indicators.Implementations
{
    public class Ema : Indicator
    {
        public Ema(List<double> values, int period)
        {
            DataSeries ds = Init(values);
            DataSeries indicator = EMA.Series(ds, period, EMACalculation.Modern);
            Values = ToIndicator(indicator);
        }
    }
}
