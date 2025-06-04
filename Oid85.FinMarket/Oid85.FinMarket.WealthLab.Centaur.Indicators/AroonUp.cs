using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class AroonUpHelper : IndicatorHelper
    {
        public override string Description { get { return @"AroonUp"; } }
        public override Type IndicatorType { get { return typeof(AroonUp); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары", "Период" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 300) }; } }
        public override string TargetPane { get { return "AroonUp"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkBlue; } }
    }

    public class AroonUp : DataSeries
    {
        public AroonUp(Bars bars, int period, string description)
            : base(bars, description)
        {
            FirstValidValue = period + 1;

            var aroonUp = new DataSeries(bars.Close - bars.Close, @"aroonUp");

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                int periodFromMax = 0;

                double max = Highest.Value(bar, bars.High, period);

                for (int i = bar - period + 1; i <= bar; i++)
                {
                    if (Math.Abs(bars.High[i] - max) < 0.00000001)
                    {
                        periodFromMax = bar - i;
                        break;
                    }
                }

                aroonUp[bar] = ((period - periodFromMax) / (double) period) * 100.0;
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = aroonUp[bar];
        }

        public static AroonUp Series(Bars bars, int period)
        {
            string description = String.Format("AroonUp({0})", period);
            if (bars.Cache.ContainsKey(description))
                return (AroonUp)bars.Cache[description];
            var result = new AroonUp(bars, period, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}