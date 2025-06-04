using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class AroonDownHelper : IndicatorHelper
    {
        public override string Description { get { return @"AroonDown"; } }
        public override Type IndicatorType { get { return typeof(AroonDown); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары", "Период" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 300) }; } }
        public override string TargetPane { get { return "AroonDown"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }
    }

    public class AroonDown : DataSeries
    {
        public AroonDown(Bars bars, int period, string description)
            : base(bars, description)
        {
            FirstValidValue = period + 1;

            var aroonDown = new DataSeries(bars.Close - bars.Close, @"AroonDown");

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                int periodFromMin = 0;

                double min = Lowest.Value(bar, bars.Low, period);

                for (int i = bar - period + 1; i <= bar; i++)
                {
                    if (Math.Abs(bars.Low[i] - min) < 0.00000001)
                    {
                        periodFromMin = bar - i;
                        break;
                    }
                }

                aroonDown[bar] = ((period - periodFromMin) / (double) period) * 100.0;
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = aroonDown[bar];
        }

        public static AroonDown Series(Bars bars, int period)
        {
            string description = String.Format("AroonDown({0})", period);
            if (bars.Cache.ContainsKey(description))
                return (AroonDown)bars.Cache[description];
            var result = new AroonDown(bars, period, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}