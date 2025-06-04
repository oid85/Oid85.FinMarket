using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.Strategies.Indicators.Implementations
{
    public class BollingerBandUpper : Indicator
    {
        public BollingerBandUpper(List<double> values, int period, double stdDev)
        {
            DataSeries ds = Init(values);
            DataSeries indicator = BBandUpper.Series(ds, period, stdDev);
            Values = ToIndicator(indicator);
        }
    }
}
