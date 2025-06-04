using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class HeikenAshiHighHelper : IndicatorHelper
    {
        public override string Description { get { return @"HeikenAshiHigh"; } }
        public override Type IndicatorType { get { return typeof(HeikenAshiHigh); } }
        public override IList<string> ParameterDescriptions { get { return new[] {@"Источник"}; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
    }

    public class HeikenAshiHigh : DataSeries
    {
        public HeikenAshiHigh(Bars bars, string description)
            : base(bars, description)
        {
            var heikenAshiHigh = new DataSeries(bars.Close - bars.Close, @"heikenAshiHigh");

            for (int bar = 1; bar < bars.Count; bar++)
            {
                heikenAshiHigh[bar] = new List<double> { bars.Open[bar], bars.Close[bar], bars.High[bar] }.Max();
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = heikenAshiHigh[bar];
        }

        public static HeikenAshiHigh Series(Bars bars)
        {
            string description = String.Format("HeikenAshiHigh");
            if (bars.Cache.ContainsKey(description))
                return (HeikenAshiHigh)bars.Cache[description];
            var result = new HeikenAshiHigh(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}