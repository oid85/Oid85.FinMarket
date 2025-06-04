using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class StochSignalHelper : IndicatorHelper
    {
        public override string Description { get { return @"StochSignal"; } }
        public override Type IndicatorType { get { return typeof(StochSignal); } }
        public override IList<string> ParameterDescriptions { get { return new[] { "Бары", "kPeriod", "dPeriod", "slowing" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars, new RangeBoundInt32(5, 2, 100), new RangeBoundInt32(3, 2, 100), new RangeBoundInt32(3, 1, 3) }; } }
        public override string TargetPane { get { return "StochSignal"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Dashed; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }
        public override string URL { get { return @""; } }
    }

    public class StochSignal : DataSeries
    {
        public StochSignal(Bars bars, int kPeriod, int dPeriod, int slowing, string description)
            : base(bars, description)
        {
            FirstValidValue = new List<int> { kPeriod, dPeriod, slowing }.Max() + 1;

            var k = new DataSeries(bars.Close - bars.Close, @"k");
            var d = new DataSeries(bars.Close - bars.Close, @"d");

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                double n = 0.0; // Числитель
                double m = 0.0; // Знаменатель

                for (int i = bar; i > bar - slowing; i--)
                {
                    n += (bars.Close[i] - Lowest.Value(i, bars.Low, kPeriod));
                    m += (Highest.Value(i, bars.High, kPeriod) - Lowest.Value(i, bars.Low, kPeriod));
                }

                k[bar] = (n / m) * 100.0;
                
            }

            d = SMA.Series(k, dPeriod);

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = d[bar];
        }

        public static StochSignal Series(Bars bars, int kPeriod, int dPeriod, int slowing)
        {
            string description = String.Format("StochSignal({0}{1}{2})", kPeriod, dPeriod, slowing);
            if (bars.Cache.ContainsKey(description))
                return (StochSignal)bars.Cache[description];
            var result = new StochSignal(bars, kPeriod, dPeriod, slowing, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}