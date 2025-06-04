using Oid85.FinMarket.Strategies.Models;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.Strategies.Indicators.Implementations
{
    public class Atr : Indicator
    {
        public Atr(List<Candle> values, int period)
        {
            Bars bars = Init(values);
            DataSeries indicator = ATR.Series(bars, period);
            Values = ToIndicator(indicator);
        }
    }
}
