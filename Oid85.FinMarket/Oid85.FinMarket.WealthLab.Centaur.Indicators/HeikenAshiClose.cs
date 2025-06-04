using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class HeikenAshiCloseHelper : IndicatorHelper
    {
        public override string Description { get { return @"HeikenAshiClose"; } }
        public override Type IndicatorType { get { return typeof(HeikenAshiClose); } }
        public override IList<string> ParameterDescriptions { get { return new[] {@"Источник"}; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
    }

    public class HeikenAshiClose : DataSeries
    {
        public HeikenAshiClose(Bars bars, string description)
            : base(bars, description)
        {
            var heikenAshiClose = new DataSeries(bars.Close - bars.Close, @"heikenAshiClose");

            for (int bar = 1; bar < bars.Count; bar++)
            {
                heikenAshiClose[bar] = (bars.Open[bar] + bars.Close[bar] + bars.High[bar] + bars.Low[bar]) / 4.0;
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = heikenAshiClose[bar];
        }

        public static HeikenAshiClose Series(Bars bars)
        {
            string description = String.Format("HeikenAshiClose");
            if (bars.Cache.ContainsKey(description))
                return (HeikenAshiClose)bars.Cache[description];
            var result = new HeikenAshiClose(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}