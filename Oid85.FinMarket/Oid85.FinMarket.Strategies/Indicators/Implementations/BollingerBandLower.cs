using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.Strategies.Indicators.Implementations
{
    public class BollingerBandLower : Indicator
    {
        public BollingerBandLower(List<double> values, int period, double stdDev)
        {
            DataSeries ds = Init(values);
            DataSeries indicator = BBandLower.Series(ds, period, stdDev);
            Values = ToIndicator(indicator);
        }
    }
}
