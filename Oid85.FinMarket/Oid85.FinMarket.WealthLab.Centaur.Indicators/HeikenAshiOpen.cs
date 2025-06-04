using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class HeikenAshiOpenHelper : IndicatorHelper
    {
        public override string Description { get { return @"HeikenAshiOpen"; } }
        public override Type IndicatorType { get { return typeof(HeikenAshiOpen); } }
        public override IList<string> ParameterDescriptions { get { return new[] {@"Источник"}; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
    }

    public class HeikenAshiOpen : DataSeries
    {
        public HeikenAshiOpen(Bars bars, string description)
            : base(bars, description)
        {
            var heikenAshiOpen = new DataSeries(bars.Close - bars.Close, @"heikenAshiOpen");

            for (int bar = 1; bar < bars.Count; bar++)
            {
                heikenAshiOpen[bar] = (bars.Open[bar - 1] + bars.Close[bar - 1]) / 2.0;
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = heikenAshiOpen[bar];
        }

        public static HeikenAshiOpen Series(Bars bars)
        {
            string description = String.Format("HeikenAshiOpen");
            if (bars.Cache.ContainsKey(description))
                return (HeikenAshiOpen)bars.Cache[description];
            var result = new HeikenAshiOpen(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}