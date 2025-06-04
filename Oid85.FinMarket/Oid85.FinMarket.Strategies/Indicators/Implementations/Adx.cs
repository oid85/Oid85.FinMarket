using Oid85.FinMarket.Strategies.Models;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.Strategies.Indicators.Implementations
{
    public class Adx : Indicator
    {
        public Adx(List<Candle> values, int period)
        {
            Bars bars = Init(values);
            DataSeries indicator = ADX.Series(bars, period);
            Values = ToIndicator(indicator);
        }
    }
}
