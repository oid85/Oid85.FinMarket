using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class NrtrHelper : IndicatorHelper
    {
        public override string Description { get {  return @"NRTR - Nick Rypock Trailing Reverse"; } }
        public override Type IndicatorType { get { return typeof(Nrtr); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары", "Период", "Множитель" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars, new RangeBoundInt32(10, 2, 20), new RangeBoundDouble(0.1, 0.1, 1),  }; } }
        public override string TargetPane { get { return "Nrtr"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkBlue; } }        
    }

    public class Nrtr : DataSeries
    {
        public Nrtr(Bars bars, int period, double mult, string description)
            : base(bars, description)
        {
            var nrtr = new DataSeries(bars.Close - bars.Close, @"Nrtr");

            double reverse = 0;
            int trend = 0;

            var currentK = bars.Close[0];
            var highPrice = bars.High[0];
            var lowPrice = bars.Low[0];

            for (int bar = 0; bar < bars.Count; bar++)
            {
                double price = bars.Close[bar];

                double prevK = currentK;

                currentK = (prevK + (price - prevK) / period) * mult;

                int newTrend = 0;

                if (trend >= 0)
                {
                    if (price > highPrice)
                        highPrice = price;

                    reverse = highPrice - currentK;

                    if (price <= reverse)
                    {
                        newTrend = -1;
                        lowPrice = price;
                        reverse = lowPrice + currentK;
                    }
                }
                if (trend <= 0)
                {
                    if (price < lowPrice)
                        lowPrice = price;

                    reverse = lowPrice + currentK;

                    if (price >= reverse)
                    {
                        newTrend = +1;
                        highPrice = price;
                        reverse = highPrice - currentK;
                    }
                }

                if (newTrend != 0)
                    trend = newTrend;

                nrtr[bar] = reverse;
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = nrtr[bar];
        }

        public static Nrtr Series(Bars bars, int period, double mult)
        {
            string description = String.Format("Nrtr({0};{1})", period, mult);
            if (bars.Cache.ContainsKey(description))
                return (Nrtr)bars.Cache[description];
            var result = new Nrtr(bars, period, mult, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}