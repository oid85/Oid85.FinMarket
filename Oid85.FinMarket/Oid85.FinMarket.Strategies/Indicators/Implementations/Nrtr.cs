using Oid85.FinMarket.Strategies.Models;
using WealthLab;

namespace Oid85.FinMarket.Strategies.Indicators.Implementations
{
    public class Nrtr : Indicator
    {
        public Nrtr(List<Candle> values, int period, double mult)
        {
            Bars bars = Init(values);
            DataSeries indicator = WealthLab.Centaur.Indicators.Nrtr.Series(bars, period, mult);
            Values = ToIndicator(indicator);
        }
    }
}
