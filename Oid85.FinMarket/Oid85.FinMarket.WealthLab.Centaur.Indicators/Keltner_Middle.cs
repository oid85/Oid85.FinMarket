using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class Keltner_MiddleHelper : IndicatorHelper
    {
        public override string Description { get { return @"Keltner_Middle"; } }
        public override Type IndicatorType { get { return typeof(Keltner_Middle); } }
        public override IList<string> ParameterDescriptions { get { return new[] { @"Бары", @"MAPeriod" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars, new RangeBoundInt32(20, 3, 200) }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
        public override string URL { get { return @"https://ru.wikipedia.org/wiki/%D0%9A%D0%B0%D0%BD%D0%B0%D0%BB_%D0%9A%D0%B5%D0%BB%D1%8C%D1%82%D0%BD%D0%B5%D1%80%D0%B0"; } }
    }

    public class Keltner_Middle : DataSeries
    {
        public Keltner_Middle(Bars bars, int maPeriod, string description)
            : base(bars, description)
        {
            FirstValidValue = maPeriod * 3;

            DataSeries middle = SMA.Series(bars.Close, maPeriod);

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = middle[bar];
        }

        public static Keltner_Middle Series(Bars bars, int maPeriod)
        {
            string description = String.Format("Keltner_Middle: {0}", maPeriod);
            if (bars.Cache.ContainsKey(description))
                return (Keltner_Middle)bars.Cache[description];
            var result = new Keltner_Middle(bars, maPeriod, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}