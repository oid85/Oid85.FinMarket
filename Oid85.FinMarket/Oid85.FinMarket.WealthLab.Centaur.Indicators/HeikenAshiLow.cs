using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class HeikenAshiLowHelper : IndicatorHelper
    {
        public override string Description { get { return @"HeikenAshiLow"; } }
        public override Type IndicatorType { get { return typeof(HeikenAshiLow); } }
        public override IList<string> ParameterDescriptions { get { return new[] {@"Источник"}; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
    }

    public class HeikenAshiLow : DataSeries
    {
        public HeikenAshiLow(Bars bars, string description)
            : base(bars, description)
        {
            var heikenAshiLow = new DataSeries(bars.Close - bars.Close, @"heikenAshiLow");

            for (int bar = 1; bar < bars.Count; bar++)
            {
                heikenAshiLow[bar] = new List<double> { bars.Open[bar], bars.Close[bar], bars.Low[bar] }.Min();
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = heikenAshiLow[bar];
        }

        public static HeikenAshiLow Series(Bars bars)
        {
            string description = String.Format("HeikenAshiLow");
            if (bars.Cache.ContainsKey(description))
                return (HeikenAshiLow)bars.Cache[description];
            var result = new HeikenAshiLow(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}