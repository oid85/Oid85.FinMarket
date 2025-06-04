using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class HMAHelper : IndicatorHelper
    {
        public override string Description { get { return @"HMA"; } }
        public override Type IndicatorType { get { return typeof (HMA); } }
        public override IList<string> ParameterDescriptions { get { return new[] {@"Источник", @"Период"}; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] {CoreDataSeries.Close, new RangeBoundInt32(20, 3, 200)}; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
        public override string URL { get { return @"https://www.instaforex.com/ru/forex_indicators/hma.php"; } }
    }

    public class HMA : DataSeries
    {
        public HMA(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            FirstValidValue = period;

            if (ds.Count < period)
                return;

            var wma1 = WMA.Series(ds, period);
            var wma2 = WMA.Series(ds, period / 2);
            var hma = WMA.Series(wma2 + (wma2 - wma1), (int) Math.Sqrt(period));

            for (int bar = 0; bar < ds.Count; bar++)
                this[bar] = hma[bar];
        }

        public static HMA Series(DataSeries ds, int period)
        {
            string description = String.Format("HMA: {0}", period);
            if (ds.Cache.ContainsKey(description))
                return (HMA)ds.Cache[description];
            var result = new HMA (ds, period, description);
            ds.Cache[description] = result;
            return result;
        }
    }
}